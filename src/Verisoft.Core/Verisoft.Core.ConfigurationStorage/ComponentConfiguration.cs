namespace Verisoft.Core.ConfigurationStorage;

public enum ConfigurationLevel
{
    Admin = 1,
    User = 2,
}

public sealed class ComponentConfiguration
{
    public Guid Id { get; set; }

    public required string Context { get; set; }

    public required string ComponentId { get; set; }

    public required string ConfigurationType { get; set; }

    public ConfigurationLevel Level { get; set; }

    public string? UserId { get; set; }

    public required string Data { get; set; }

    public int SchemaVersion { get; set; } = 1;

    public DateTimeOffset UpdatedAt { get; set; }
}