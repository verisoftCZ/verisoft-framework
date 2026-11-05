using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.ConfigurationStorage;

internal sealed class ComponentConfigurationStorage<TContext>(
    TContext dbContext,
    IUserContext userContext,
    IAuthorizationService authorization,
    IOptions<ConfigurationStorageOptions> options,
    TimeProvider timeProvider) : IConfigurationStorage
    where TContext : DbContext
{
    private const int QueryBatchSize = 500;
    private readonly ConfigurationStorageOptions options = options.Value;

    public async Task<IReadOnlyList<ComponentConfigurationsResponse>> GetAsync(IReadOnlyCollection<ConfigurationKey> keys, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        foreach (var key in keys)
        {
            ValidateKey(key);
        }

        if (keys.Count == 0)
        {
            return [];
        }

        var requestedKeys = keys.Distinct().ToArray();
        var query = dbContext.Set<ComponentConfiguration>().AsNoTracking()
            .Where(configuration => configuration.Context == options.Context
                && (configuration.Level == ConfigurationLevel.Admin || (configuration.Level == ConfigurationLevel.User && configuration.UserId == userId)));
        var configurations = new List<ComponentConfiguration>();
        for (var index = 0; index < requestedKeys.Length; index += QueryBatchSize)
        {
            var keyBatch = requestedKeys.Skip(index).Take(QueryBatchSize).ToArray();
            configurations.AddRange(await query.Where(BuildKeyPredicate(keyBatch)).ToListAsync(cancellationToken));
        }

        var configurationsByKey = configurations.ToDictionary(configuration =>
            (configuration.ComponentId, configuration.ConfigurationType, configuration.Level));

        return keys.Select(key => new ComponentConfigurationsResponse(
            key.ComponentId,
            key.ConfigurationType,
            configurationsByKey.TryGetValue((key.ComponentId, key.ConfigurationType, ConfigurationLevel.Admin), out var admin) ? ToResponse(admin) : null,
            configurationsByKey.TryGetValue((key.ComponentId, key.ConfigurationType, ConfigurationLevel.User), out var user) ? ToResponse(user) : null))
            .ToArray();
    }

    public async Task<ConfigurationResponse> UpsertAsync(ConfigurationKey key, ConfigurationLevel level, SaveConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        var owner = await GetOwnerAsync(level);
        ValidateKey(key);
        Validator.ValidateObject(request, new ValidationContext(request), true);
        var configuration = await Find(key, level, owner).SingleOrDefaultAsync(cancellationToken);
        var isNew = configuration is null;
        if (configuration is null)
        {
            configuration = new ComponentConfiguration
            {
                Id = Guid.NewGuid(),
                Context = options.Context,
                ComponentId = key.ComponentId,
                ConfigurationType = key.ConfigurationType,
                Level = level,
                UserId = owner,
                Data = request.Data.GetRawText(),
            };
            dbContext.Add(configuration);
        }

        Apply(configuration, request);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException) when (isNew)
        {
            dbContext.Entry(configuration).State = EntityState.Detached;
            var concurrent = await Find(key, level, owner).SingleOrDefaultAsync(cancellationToken);
            if (concurrent is null)
            {
                throw;
            }

            Apply(concurrent, request);
            await dbContext.SaveChangesAsync(cancellationToken);
            configuration = concurrent;
        }

        return ToResponse(configuration)!;
    }

    public async Task<bool> DeleteAsync(ConfigurationKey key, ConfigurationLevel level, CancellationToken cancellationToken = default)
    {
        var owner = await GetOwnerAsync(level);
        ValidateKey(key);
        return await Find(key, level, owner).ExecuteDeleteAsync(cancellationToken) > 0;
    }

    private IQueryable<ComponentConfiguration> Find(ConfigurationKey key, ConfigurationLevel level, string? owner)
        => dbContext.Set<ComponentConfiguration>().Where(configuration => configuration.Context == options.Context
            && configuration.ComponentId == key.ComponentId && configuration.ConfigurationType == key.ConfigurationType
            && configuration.Level == level && configuration.UserId == owner);

    private async Task<string?> GetOwnerAsync(ConfigurationLevel level)
    {
        var userId = GetUserId();
        if (level is not ConfigurationLevel.Admin and not ConfigurationLevel.User)
        {
            throw new ValidationException("Unsupported configuration level.");
        }

        if (level == ConfigurationLevel.User)
        {
            return userId;
        }

        var result = await authorization.AuthorizeAsync((ClaimsPrincipal)userContext.User, null, ConfigurationStorageOptions.AdminPolicy);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Administrator access is required.");
        }

        return null;
    }

    private string GetUserId()
    {
        if (userContext.User is not ClaimsPrincipal { Identity.IsAuthenticated: true } principal)
        {
            throw new UnauthorizedAccessException("An authenticated identity is required.");
        }

        var userId = principal.FindFirst(options.UserIdClaimType)?.Value;
        if (string.IsNullOrWhiteSpace(userId)
            || userId.Length > 200
            || !string.Equals(userId, userId.Trim(), StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException("A stable user identifier is required.");
        }

        return userId;
    }

    private static void ValidateKey(ConfigurationKey key)
    {
        if (key is null)
        {
            throw new ValidationException("Configuration keys must not be null.");
        }

        Validator.ValidateObject(key, new ValidationContext(key), true);
        if (!string.Equals(key.ComponentId, key.ComponentId.Trim(), StringComparison.Ordinal)
            || !string.Equals(key.ConfigurationType, key.ConfigurationType.Trim(), StringComparison.Ordinal))
        {
            throw new ValidationException("Configuration keys must not have leading or trailing whitespace.");
        }
    }

    private void Apply(ComponentConfiguration configuration, SaveConfigurationRequest request)
    {
        configuration.Data = request.Data.GetRawText();
        configuration.SchemaVersion = request.SchemaVersion;
        configuration.UpdatedAt = timeProvider.GetUtcNow();
        var entry = dbContext.Entry(configuration);
        if (entry.State != EntityState.Added)
        {
            entry.Property(value => value.Data).IsModified = true;
            entry.Property(value => value.SchemaVersion).IsModified = true;
            entry.Property(value => value.UpdatedAt).IsModified = true;
        }
    }

    private static Expression<Func<ComponentConfiguration, bool>> BuildKeyPredicate(IReadOnlyCollection<ConfigurationKey> keys)
    {
        var configuration = Expression.Parameter(typeof(ComponentConfiguration), "configuration");
        var componentId = Expression.Property(configuration, nameof(ComponentConfiguration.ComponentId));
        var configurationType = Expression.Property(configuration, nameof(ComponentConfiguration.ConfigurationType));
        Expression? predicate = null;
        foreach (var key in keys)
        {
            var matchesKey = Expression.AndAlso(
                Expression.Equal(componentId, Capture(key.ComponentId)),
                Expression.Equal(configurationType, Capture(key.ConfigurationType)));
            predicate = predicate is null ? matchesKey : Expression.OrElse(predicate, matchesKey);
        }

        return Expression.Lambda<Func<ComponentConfiguration, bool>>(predicate!, configuration);
    }

    private static Expression Capture<TValue>(TValue value)
    {
        Expression<Func<TValue>> accessor = () => value;
        return accessor.Body;
    }

    private static ConfigurationResponse? ToResponse(ComponentConfiguration? configuration)
        => configuration is null ? null : new ConfigurationResponse(configuration.Id, configuration.Level,
            JsonSerializer.Deserialize<JsonElement>(configuration.Data), configuration.SchemaVersion, configuration.UpdatedAt);
}