namespace Verisoft.Core.Codebooks.Common.Models.Bulk;

public class BulkImportStringValueResult(string stringValue)
{
    public string StringValue { get; init; } = stringValue;

    public List<string> Errors { get; set; } = [];
}