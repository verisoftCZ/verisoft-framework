namespace Verisoft.CodebookApi.Contracts.Filters;

public class CodebookFilter
{
    public string? Name { get; set; }

    public bool? IsBasicType { get; set; }

    public bool? HasHardcodedEnum { get; set; }
}