using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Codebooks.Common.Models;

public class CodebookRecordListData<TCodebookRecordType> : PagedData<CodebookRecord<TCodebookRecordType>>
    where TCodebookRecordType : CodebookRecordType;
