using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Common.Constants;

public static class CodebookRecordTranslationEntityConstants
{
    public const int ValueMaxLength = 500;
    public const int LanguageCodeMaxLength = 10;

    public const string Value = nameof(CodebookRecordTranslationEntity<MarkerCodebookType>.Value);
    public const string LanguageCode = nameof(CodebookRecordTranslationEntity<MarkerCodebookType>.LanguageCode);
    public const string CodebookRecordId = nameof(CodebookRecordTranslationEntity<MarkerCodebookType>.CodebookRecordId);
    public const string CodebookRecord = nameof(CodebookRecordTranslationEntity<MarkerCodebookType>.CodebookRecord);
    public const string Id = nameof(CodebookRecordTranslationEntity<MarkerCodebookType>.Id);
}
