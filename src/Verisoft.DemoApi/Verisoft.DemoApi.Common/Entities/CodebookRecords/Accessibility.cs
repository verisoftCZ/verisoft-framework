using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.DemoApi.Common.Entities.CodebookRecords;

public class Accessibility : CodebookRecordType
{
    public static CodebookRecord<Accessibility> Public => new("Public");

    public static CodebookRecord<Accessibility> Internal => new("Internal");

    public static CodebookRecord<Accessibility> Restricted => new("Restricted");
}