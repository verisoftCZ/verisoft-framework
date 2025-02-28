namespace Verisoft.CodebookApi.Contracts.Filters;

public class StringValueDetailFilter
{
    public string? SearchTerm { get; set; }

    public bool? HasExceptionalValues { get; set; }

    public int? TenantId { get; set; }
}