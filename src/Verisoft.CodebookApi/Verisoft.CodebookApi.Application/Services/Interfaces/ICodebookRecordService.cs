using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.CodebookRecord;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface ICodebookRecordService
{
    Task<BusinessActionResult<CodebookRecordListData<TAdditionalProperties>>> GetCodebookRecordsAsync<TAdditionalProperties, TTenantValue>(Verisoft.Core.Contracts.FilteredRequest<CodebookRecordFilter> request)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<CodebookRecord<TAdditionalProperties>>> GetCodebookRecordAsync<TAdditionalProperties, TTenantValue>(string stringValue, string? languageCode)
    where TAdditionalProperties : class, new()
    where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<bool>> IsCodebookRecordAvailableAsync<TAdditionalProperties, TTenantValue>(string stringValue)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new();
}
