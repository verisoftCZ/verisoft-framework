using Verisoft.CodebookApi.Contracts.Models.Base;

namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class StringValueDetail<TAdditionalProperties, TTenantValue> : StringValueDetailEditModel<TAdditionalProperties, TTenantValue>
    where TAdditionalProperties : class
    where TTenantValue : ITenantValue
{
    public required string StringValue { get; set; }

    public bool IsGlobal { get; set; }
}