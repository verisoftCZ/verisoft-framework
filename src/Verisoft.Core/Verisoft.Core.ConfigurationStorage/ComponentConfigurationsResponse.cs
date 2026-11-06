namespace Verisoft.Core.ConfigurationStorage;

/// <summary>Both available levels; null means that the level has no stored configuration. No merging is performed.</summary>
public sealed record ComponentConfigurationsResponse(string ComponentId, string ConfigurationType, ConfigurationResponse? Admin, ConfigurationResponse? User);
