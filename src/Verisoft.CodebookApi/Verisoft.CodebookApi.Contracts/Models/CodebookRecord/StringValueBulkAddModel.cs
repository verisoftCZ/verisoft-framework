using Verisoft.CodebookApi.Contracts.Models.Base;

namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class StringValueBulkAddModel<TAdditionalProperties, TTenantValue>
    where TAdditionalProperties : class
    where TTenantValue : ITenantValue
{
    public List<StringValueDefaultTranslation> StringValues { get; set; } = [];
}
