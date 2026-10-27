namespace Verisoft.Core.Codebooks.Common.Models;

public class StringValueDetail<TAdditionalProperties> : StringValueDetailCreateModel<TAdditionalProperties>
    where TAdditionalProperties : class
{
    public required int Id { get; init; }

    public bool IsValid { get; init; }
}