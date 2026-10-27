namespace Verisoft.Core.Codebooks.Configurations;

public class MainCodebookRecordConfiguration
{
    public List<CodebookRecordConfiguration> CodebookRecordConfiguration { get; set; } = [];

    public string CodebookRecordTablePrefix { get; set; } = string.Empty;

    public string CodebookRecordRoutePrefix { get; set; } = string.Empty;
}
