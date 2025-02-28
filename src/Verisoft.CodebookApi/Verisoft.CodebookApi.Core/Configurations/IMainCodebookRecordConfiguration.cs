namespace Verisoft.CodebookApi.Core.Configurations;

public interface IMainCodebookRecordConfiguration
{
    public List<CodebookRecordConfiguration> CodebookRecordConfiguration { get; set; }

    public string CodebookRecordTablePrefix { get; set; }

    public string CodebookRecordRoutePrefix { get; set; }
}