using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Entities.BaseEntity;

public class CodebookRecordEntity : BaseEntity<int>, ICodebookRecordEntity
{
    public string StringValue { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDefault { get; set; }

    public bool IsGlobal { get; set; }

    public HashSet<TranslationEntity>? Translations { get; set; }

    public HashSet<TenantValueEntity>? TenantValues { get; set; }
}