using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;
using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Contracts.Models.CodebookParent;

public class CodebookParentResponse
{
    public required int Id { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public required CodebookRecord<Accessibility> Accessibility { get; init; }

    public bool IsActive { get; set; }

    public CodebookParentType Type { get; set; }

    public string? ClientName { get; set; }
}
