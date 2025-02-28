using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Entities;

public class LanguageEntity : BaseEntity<int>
{
    public required string Name { get; set; }

    public required string Code { get; set; }

    public bool IsDefault { get; set; }
}
