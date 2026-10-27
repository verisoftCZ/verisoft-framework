using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.Models;

public class CodebookRecordWithTranslation<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
{
    public required CodebookRecordEntity<TCodebookRecordType> Record { get; set; }

    public required string Translation { get; set; }
}