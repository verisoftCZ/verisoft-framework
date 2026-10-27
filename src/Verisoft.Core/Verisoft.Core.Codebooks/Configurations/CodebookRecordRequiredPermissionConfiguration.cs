namespace Verisoft.Core.Codebooks.Configurations;

public class CodebookRecordRequiredPermissionConfiguration
{
    public List<string> GlobalEditPermissions { get; set; } = [];

    public List<string> GlobalReadPermissions { get; set; } = [];

    public Dictionary<string, List<string>> CustomPermissions { get; set; } = [];
}