namespace Verisoft.Core.Codebooks.Common.Models;

public class StringValueTranslations
{
    public required string StringValue { get; set; }

    public Translation[] Translations { get; set; } = [];
}