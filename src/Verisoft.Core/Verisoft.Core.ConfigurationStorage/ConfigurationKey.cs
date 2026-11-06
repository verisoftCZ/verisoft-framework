using System.ComponentModel.DataAnnotations;

namespace Verisoft.Core.ConfigurationStorage;

/// <summary>A component and configuration type to look up.</summary>
public sealed record ConfigurationKey
{
    [Required, StringLength(200)]
    public required string ComponentId { get; init; }

    [Required, StringLength(100)]
    public required string ConfigurationType { get; init; }
}
