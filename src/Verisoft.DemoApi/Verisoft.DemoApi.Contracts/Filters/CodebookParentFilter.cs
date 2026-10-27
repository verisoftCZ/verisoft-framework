using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;

namespace Verisoft.DemoApi.Contracts.Filters;

public sealed class CodebookParentFilter
{
    public CodebookRecord<Accessibility>[] Accessibility { get; init; } = [];
}
