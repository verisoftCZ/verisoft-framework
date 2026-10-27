using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Common.Constants;

public static class CodebookRecordEntityConstants
{
    public const int StringValueMaxLength = 50;
    public const int DescriptionValueMaxLength = 255;
    public const string CodebookRecordForeignKeyPropertySuffix = "StringValue";
    public const string CodebookRecordEntityPropertySuffix = "CodebookRecord";

    public const string AdditionalProperties = nameof(CodebookRecordEntity<MarkerCodebookType>.AdditionalProperties);
    public const string StringValue = nameof(CodebookRecordEntity<MarkerCodebookType>.StringValue);
    public const string Translations = nameof(CodebookRecordEntity<MarkerCodebookType>.Translations);
    public const string Description = nameof(CodebookRecordEntity<MarkerCodebookType>.Description);
    public const string IsForbidden = nameof(CodebookRecordEntity<MarkerCodebookType>.IsValid);
    public const string Id = nameof(CodebookRecordEntity<MarkerCodebookType>.Id);
}
