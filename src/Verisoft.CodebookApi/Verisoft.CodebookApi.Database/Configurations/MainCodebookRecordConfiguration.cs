using Verisoft.CodebookApi.Core.Configurations;

namespace Verisoft.CodebookApi.Database.Configurations;

public class MainCodebookRecordConfiguration : IMainCodebookRecordConfiguration
{
    public List<CodebookRecordConfiguration> CodebookRecordConfiguration { get; set; } = [];

    public string CodebookRecordTablePrefix { get; set; } = string.Empty;

    public string CodebookRecordRoutePrefix { get; set; } = string.Empty;
}
