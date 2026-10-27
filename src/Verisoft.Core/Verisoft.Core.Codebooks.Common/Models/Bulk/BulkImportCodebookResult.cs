namespace Verisoft.Core.Codebooks.Common.Models.Bulk;

public class BulkImportCodebookResult(string codebookName)
{
    public string CodebookName { get; init; } = codebookName;

    public string? Error { get; set; }

    public List<BulkImportStringValueResult> StringValues { get; set; } = [];
}