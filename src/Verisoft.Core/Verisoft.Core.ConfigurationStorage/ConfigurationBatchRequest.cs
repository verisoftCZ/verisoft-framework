using System.ComponentModel.DataAnnotations;

namespace Verisoft.Core.ConfigurationStorage;

/// <summary>Batch lookup of component/type pairs.</summary>
public sealed record ConfigurationBatchRequest
{
    [Required]
    public required ConfigurationKey[] Configurations { get; init; }
}
