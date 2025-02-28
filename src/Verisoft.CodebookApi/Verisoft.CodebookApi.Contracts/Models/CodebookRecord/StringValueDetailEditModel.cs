using Verisoft.CodebookApi.Contracts.Models.Base;

namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class StringValueDetailEditModel<TAdditionalProperties, TTenantValue>
    where TAdditionalProperties : class
    where TTenantValue : ITenantValue
{
    public string? Description { get; set; }

    public TAdditionalProperties? DefaultAdditionalProperties { get; set; }

    public List<TTenantValue>? TenantValues { get; set; }

    public required string DefaultTranslation { get; set; }
}