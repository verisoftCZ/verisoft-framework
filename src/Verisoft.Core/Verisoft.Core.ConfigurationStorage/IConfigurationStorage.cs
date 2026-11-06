using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Verisoft.Core.ConfigurationStorage;

public interface IConfigurationStorage
{
    Task<IReadOnlyList<ComponentConfigurationsResponse>> GetAsync(IReadOnlyCollection<ConfigurationKey> keys, CancellationToken cancellationToken = default);

    Task<ConfigurationResponse> UpsertAsync(ConfigurationKey key, ConfigurationLevel level, SaveConfigurationRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(ConfigurationKey key, ConfigurationLevel level, CancellationToken cancellationToken = default);
}