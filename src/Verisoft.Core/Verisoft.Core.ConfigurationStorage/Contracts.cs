using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Verisoft.Core.ConfigurationStorage;

/// <summary>Opaque JSON content and its frontend-owned schema version.</summary>
public sealed record SaveConfigurationRequest : IValidatableObject
{
    public required JsonElement Data { get; init; }

    [Range(1, int.MaxValue)]
    public int SchemaVersion { get; init; } = 1;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Data.ValueKind == JsonValueKind.Undefined)
        {
            yield return new ValidationResult("Data must contain a JSON value.", [nameof(Data)]);
        }
    }
}

/// <summary>A component and configuration type to look up.</summary>
public sealed record ConfigurationKey
{
    [Required, StringLength(200)]
    public required string ComponentId { get; init; }

    [Required, StringLength(100)]
    public required string ConfigurationType { get; init; }
}

/// <summary>Batch lookup of component/type pairs.</summary>
public sealed record ConfigurationBatchRequest
{
    [Required]
    public required ConfigurationKey[] Configurations { get; init; }
}

/// <summary>A stored configuration without persistence or ownership metadata.</summary>
public sealed record ConfigurationResponse(Guid Id, ConfigurationLevel Level, JsonElement Data, int SchemaVersion, DateTimeOffset UpdatedAt);

/// <summary>Both available levels; null means that the level has no stored configuration. No merging is performed.</summary>
public sealed record ComponentConfigurationsResponse(string ComponentId, string ConfigurationType, ConfigurationResponse? Admin, ConfigurationResponse? User);

public interface IConfigurationStorage
{
    Task<IReadOnlyList<ComponentConfigurationsResponse>> GetAsync(IReadOnlyCollection<ConfigurationKey> keys, CancellationToken cancellationToken = default);

    Task<ConfigurationResponse> UpsertAsync(ConfigurationKey key, ConfigurationLevel level, SaveConfigurationRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(ConfigurationKey key, ConfigurationLevel level, CancellationToken cancellationToken = default);
}