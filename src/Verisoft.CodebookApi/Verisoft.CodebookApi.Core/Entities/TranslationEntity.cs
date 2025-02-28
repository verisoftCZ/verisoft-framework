using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Entities;

public class TranslationEntity : BaseEntity<int>
{
    public required string TableName { get; set; }

    public required string TableStringValue { get; set; }

    public required string Value { get; set; }

    public int LanguageId { get; set; }

    public required LanguageEntity Language { get; set; }
}
