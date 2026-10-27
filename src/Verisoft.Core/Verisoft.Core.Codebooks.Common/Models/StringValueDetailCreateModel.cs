namespace Verisoft.Core.Codebooks.Common.Models;

public class StringValueDetailCreateModel<TAdditionalProperties> : StringValueDetailEditModel<TAdditionalProperties>
    where TAdditionalProperties : class
{
    public required string StringValue { get; init; }
}