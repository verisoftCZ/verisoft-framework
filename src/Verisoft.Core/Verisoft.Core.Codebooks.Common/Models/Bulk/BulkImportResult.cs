namespace Verisoft.Core.Codebooks.Common.Models.Bulk;

public class BulkImportResult
{
    public BulkImportResult()
    {
    }

    public BulkImportResult(string error)
    {
        Errors = [error];
    }

    public List<string> Errors { get; set; } = [];

    public List<BulkImportCodebookResult> Codebooks { get; set; } = [];

    public bool Success => Errors.Count == 0 && Codebooks.All(x => x.Error is null && x.StringValues.All(x => x.Errors.Count == 0));
}