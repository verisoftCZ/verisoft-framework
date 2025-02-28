using Verisoft.Core.Contracts;

namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class CodebookRecordListData<TAdditionalProperties> : PagedData<CodebookRecord<TAdditionalProperties>>
    where TAdditionalProperties : class
{
}
