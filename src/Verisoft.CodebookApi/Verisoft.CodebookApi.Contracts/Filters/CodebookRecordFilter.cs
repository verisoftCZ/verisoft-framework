namespace Verisoft.CodebookApi.Contracts.Filters;

public class CodebookRecordFilter
{
    public string? Translation { get; set; }

    public string? LanguageCode { get; set; }

    public string[]? StringValueArray { get; set; }

    public string[]? StringValueArrayToExclude { get; set; }
}
