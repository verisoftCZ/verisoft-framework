namespace Verisoft.Core.Codebooks.Common.Models;

public class CodebookEditModel
{
    public required string Name { get; set; }

    public required string Code { get; set; }

    public bool IsExternal { get; set; }
}
