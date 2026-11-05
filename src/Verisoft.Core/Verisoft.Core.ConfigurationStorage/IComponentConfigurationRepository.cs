using Verisoft.Core.Common.Store;

namespace Verisoft.Core.ConfigurationStorage;

public interface IComponentConfigurationRepository : IRepository<ComponentConfiguration, Guid>
{
    Task<IReadOnlyList<ComponentConfiguration>> GetAvailableAsync(string context, string userId, IReadOnlyCollection<ConfigurationKey> keys, CancellationToken cancellationToken = default);

    Task<ComponentConfiguration?> FindAsync(string context, ConfigurationKey key, ConfigurationLevel level, string? owner, CancellationToken cancellationToken = default);

    Task<ComponentConfiguration> UpsertAsync(ComponentConfiguration replacement, CancellationToken cancellationToken = default);
}