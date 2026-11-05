using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Data.EntityFramework;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.Core.ConfigurationStorage;

public sealed class ComponentConfigurationRepository(EntityFrameworkDbContextBase dbContext)
    : EntityFrameworkRepositoryBase<ComponentConfiguration, Guid>(dbContext), IComponentConfigurationRepository
{
    private const int QueryBatchSize = 500;

    public async Task<IReadOnlyList<ComponentConfiguration>> GetAvailableAsync(string context, string userId, IReadOnlyCollection<ConfigurationKey> keys, CancellationToken cancellationToken = default)
    {
        var requestedKeys = keys.Distinct().ToArray();
        var query = GetQueryable().AsNoTracking()
            .Where(configuration => configuration.Context == context
                && (configuration.Level == ConfigurationLevel.Admin || (configuration.Level == ConfigurationLevel.User && configuration.UserId == userId)));
        var configurations = new List<ComponentConfiguration>();
        for (var index = 0; index < requestedKeys.Length; index += QueryBatchSize)
        {
            var keyBatch = requestedKeys.Skip(index).Take(QueryBatchSize).ToArray();
            configurations.AddRange(await query.Where(BuildKeyPredicate(keyBatch)).ToListAsync(cancellationToken));
        }

        return configurations;
    }

    public Task<ComponentConfiguration?> FindAsync(string context, ConfigurationKey key, ConfigurationLevel level, string? owner, CancellationToken cancellationToken = default)
        => GetQueryable().SingleOrDefaultAsync(configuration => configuration.Context == context
            && configuration.ComponentId == key.ComponentId && configuration.ConfigurationType == key.ConfigurationType
            && configuration.Level == level && configuration.UserId == owner, cancellationToken);

    public async Task<ComponentConfiguration> UpsertAsync(ComponentConfiguration replacement, CancellationToken cancellationToken = default)
    {
        var key = new ConfigurationKey { ComponentId = replacement.ComponentId, ConfigurationType = replacement.ConfigurationType };
        var configuration = await FindAsync(replacement.Context, key, replacement.Level, replacement.UserId, cancellationToken);
        var isNew = configuration is null;
        if (configuration is null)
        {
            configuration = replacement;
            await AddAsync(configuration);
        }
        else
        {
            Apply(configuration, replacement);
        }

        try
        {
            await UnitOfWork.CommitAsync(cancellationToken);
        }
        catch (DbUpdateException) when (isNew)
        {
            dbContext.Entry(configuration).State = EntityState.Detached;
            var concurrent = await FindAsync(replacement.Context, key, replacement.Level, replacement.UserId, cancellationToken);
            if (concurrent is null)
            {
                throw;
            }

            Apply(concurrent, replacement);
            await UnitOfWork.CommitAsync(cancellationToken);
            configuration = concurrent;
        }

        return configuration;
    }

    private void Apply(ComponentConfiguration configuration, ComponentConfiguration replacement)
    {
        configuration.Data = replacement.Data;
        configuration.SchemaVersion = replacement.SchemaVersion;
        var entry = dbContext.Entry(configuration);
        entry.Property(value => value.Data).IsModified = true;
        entry.Property(value => value.SchemaVersion).IsModified = true;
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
}