namespace Verisoft.Core.Codebooks.Common.Models.Bulk;

public class StringValueImportRow
{
    public string StringValue { get; set; } = string.Empty;

    public string? ValidFrom { get; set; }

    public string? ValidTo { get; set; }

    public string? Description { get; set; }

    public Dictionary<string, string> TranslationDictionary { get; set; } = [];

    public Dictionary<string, string> AdditionalPropertiesDictionary { get; set; } = [];
}