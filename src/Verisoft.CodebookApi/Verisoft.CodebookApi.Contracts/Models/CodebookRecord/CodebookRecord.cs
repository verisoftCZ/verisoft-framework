namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class CodebookRecord<TAdditionalProperties>
    where TAdditionalProperties : class
{
    public required string StringValue { get; set; }

    public bool IsForbidden { get; set; }

    public required string Translation { get; set; }

    public TAdditionalProperties? AdditionalProperties { get; set; }
}
