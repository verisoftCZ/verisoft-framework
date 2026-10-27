using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Codebooks.Common.Entities;

public class CodebookEntity : BaseEntity<int>
{
    public required string Name { get; set; }

    public required string Code { get; set; }

    public bool IsExternal { get; set; }
}