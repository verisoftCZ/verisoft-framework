namespace Verisoft.Core.Codebooks.Common.Models;

public class StringValueDetailEditModel<TAdditionalProperties>
    where TAdditionalProperties : class
{
    public string? Description { get; set; }

    public TAdditionalProperties? AdditionalProperties { get; set; }

    public required string DefaultTranslation { get; set; }

    public DateOnly ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }
}