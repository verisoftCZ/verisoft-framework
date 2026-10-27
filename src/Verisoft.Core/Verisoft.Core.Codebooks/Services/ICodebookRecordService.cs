using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

public interface ICodebookRecordService<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
{
    Task<BusinessActionResult<CodebookRecordListData<TCodebookRecordType>>> GetCodebookRecordsAsync(FilteredRequest<CodebookRecordFilter> request);

    Task<BusinessActionResult<CodebookRecord<TCodebookRecordType>>> GetCodebookRecordAsync(string stringValue, string? languageCode);

    Task<BusinessActionResult<bool>> IsCodebookRecordAvailableAsync(string stringValue);
}