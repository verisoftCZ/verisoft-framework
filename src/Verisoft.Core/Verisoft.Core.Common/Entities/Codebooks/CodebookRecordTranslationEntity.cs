namespace Verisoft.Core.Common.Entities.Codebooks;

public class CodebookRecordTranslationEntity<TCodebookRecordType> : BaseEntity<int>
    where TCodebookRecordType : CodebookRecordType, new()
{
    public required string Value { get; set; }

    public required string LanguageCode { get; set; }

    public int CodebookRecordId { get; set; }

    public required CodebookRecordEntity<TCodebookRecordType> CodebookRecord { get; set; }
}