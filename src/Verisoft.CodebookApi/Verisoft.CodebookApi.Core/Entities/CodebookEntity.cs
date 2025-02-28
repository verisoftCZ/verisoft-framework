using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Entities;

public class CodebookEntity : BaseEntity<int>
{
    public required string Name { get; set; }

    public bool IsBasicType { get; set; }

    public bool HasHardcodedEnum { get; set; }

    public bool IsExternal { get; set; }
}
