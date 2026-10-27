namespace Verisoft.Core.Codebooks.Common.Filters;

public class CodebookRecordFilter
{
    public string? Translation { get; set; }

    public string? LanguageCode { get; set; }

    public string[]? StringValue { get; set; }

    public string[]? Exclude { get; set; }
}
