namespace Verisoft.CodebookApi.Core.Configurations;

public class CodebookRecordConfiguration
{
    private string? codebookRecordTableName = null;
    private string? codebookRecordEndpointRoute = null;

    public required Type CodebookRecordType { get; set; }

    public required Type PropContract { get; set; }

    public required Type TenantContract { get; set; }

    public string CodebookRecordTableName
    {
        get => codebookRecordTableName ?? CodebookRecordType.Name.Replace("Entity", string.Empty);
        set => codebookRecordTableName = value;
    }

    public string CodebookRecordEndpointName
    {
        get => codebookRecordEndpointRoute ?? CodebookRecordType.Name.Replace("Entity", string.Empty);
        set => codebookRecordEndpointRoute = value;
    }
}
