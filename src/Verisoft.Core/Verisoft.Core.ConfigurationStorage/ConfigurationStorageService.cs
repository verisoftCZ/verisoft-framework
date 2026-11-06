using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.ConfigurationStorage;

public sealed class ConfigurationStorageService(
    IComponentConfigurationRepository repository,
    IUserContext userContext,
    IAuthorizationService authorization,
    IOptions<ConfigurationStorageOptions> options) : IConfigurationStorage
{
    private readonly ConfigurationStorageOptions options = options.Value;

    public async Task<IReadOnlyList<ComponentConfigurationsResponse>> GetAsync(IReadOnlyCollection<ConfigurationKey> keys, CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        foreach (var key in keys)
        {
            ValidateKey(key);
        }

        var configurations = await repository.GetAvailableAsync(options.Context, userId, keys, cancellationToken);
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
        var configuration = await repository.UpsertAsync(new ComponentConfiguration
        {
            Id = Guid.NewGuid(),
            Context = options.Context,
            ComponentId = key.ComponentId,
            ConfigurationType = key.ConfigurationType,
            Level = level,
            UserId = owner,
            Data = request.Data.GetRawText(),
            SchemaVersion = request.SchemaVersion,
        }, cancellationToken);
        return ToResponse(configuration);
    }

    public async Task<bool> DeleteAsync(ConfigurationKey key, ConfigurationLevel level, CancellationToken cancellationToken = default)
    {
        var owner = await GetOwnerAsync(level);
        ValidateKey(key);
        var configuration = await repository.FindAsync(options.Context, key, level, owner, cancellationToken);
        if (configuration is null)
        {
            return false;
        }

        return await repository.DeleteAsync(configuration, cancellationToken);
    }

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
        var userId = userContext.User.GetUserId(options.UserIdClaimType);
        if (userId.Length > 200
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

    private static ConfigurationResponse ToResponse(ComponentConfiguration configuration)
        => new(configuration.Id, configuration.Level,
            JsonSerializer.Deserialize<JsonElement>(configuration.Data), configuration.SchemaVersion,
            new DateTimeOffset(DateTime.SpecifyKind(configuration.UpdatedAt ?? configuration.CreatedAt!.Value, DateTimeKind.Utc)));
}