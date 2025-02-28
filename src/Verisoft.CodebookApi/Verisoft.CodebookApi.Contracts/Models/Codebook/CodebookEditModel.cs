namespace Verisoft.CodebookApi.Contracts.Models.Codebook;

public class CodebookEditModel
{
    public required string Name { get; set; }

    public bool IsBasicType { get; set; }

    public bool HasHardcodedEnum { get; set; }

    public bool IsExternal { get; set; }
}
