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
