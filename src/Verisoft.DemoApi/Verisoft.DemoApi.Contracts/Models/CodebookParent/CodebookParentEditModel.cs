using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;

namespace Verisoft.DemoApi.Contracts.Models.CodebookParent;

public class CodebookParentEditModel
{
    public required string Code { get; set; }

    public required string Name { get; set; }

    public required CodebookRecord<Accessibility> Accessibility { get; init; }

    public int? ClientId { get; set; }
}
