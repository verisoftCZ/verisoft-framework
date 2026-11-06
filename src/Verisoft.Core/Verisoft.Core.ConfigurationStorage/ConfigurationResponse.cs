using System.Text.Json;

namespace Verisoft.Core.ConfigurationStorage;

/// <summary>A stored configuration without persistence or ownership metadata.</summary>
public sealed record ConfigurationResponse(Guid Id, ConfigurationLevel Level, JsonElement Data, int SchemaVersion, DateTimeOffset UpdatedAt);
