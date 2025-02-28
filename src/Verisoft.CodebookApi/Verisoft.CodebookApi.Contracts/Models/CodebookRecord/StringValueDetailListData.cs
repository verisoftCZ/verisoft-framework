using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.Core.Contracts;

namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class StringValueDetailListData<TPropertyContract, TTenantValue> : PagedData<StringValueDetail<TPropertyContract, TTenantValue>>
        where TPropertyContract : class, new()
        where TTenantValue : class, ITenantValue, new()
{
}
