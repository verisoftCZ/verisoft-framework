using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Entities.BaseEntity;

public interface ICodebookRecordEntity : IBaseEntity<int>
{
    public string StringValue { get; set; }

    public string? Description { get; set; }

    public bool IsDefault { get; set; }

    public bool IsGlobal { get; set; }

    public HashSet<TranslationEntity>? Translations { get; set; }

    public HashSet<TenantValueEntity>? TenantValues { get; set; }
}
