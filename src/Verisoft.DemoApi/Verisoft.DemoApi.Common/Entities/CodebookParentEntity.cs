using Verisoft.Core.Codebooks.Common.Attributes;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;
using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Common.Entities;

public partial class CodebookParentEntity : BaseEntity<int>
{
    public required string Code { get; set; }

    public required string Name { get; set; }

    [GenerateCodebookInfrastructure]
    public required CodebookRecord<Accessibility> Accessibility { get => AccessibilityStringValue!; set => AccessibilityStringValue = value!; }

    public bool IsActive { get; set; }

    public CodebookParentType Type { get; set; } = CodebookParentType.Irrelevant;

    public int? ClientId { get; set; }

    public ClientEntity? Client { get; set; }
}
