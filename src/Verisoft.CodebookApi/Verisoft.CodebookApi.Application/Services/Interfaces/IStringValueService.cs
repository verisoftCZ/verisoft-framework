using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.CodebookRecord;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface IStringValueService
{
    Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> GetStringValueDetailAsync<TAdditionalProperties, TTenantValue>(string stringValue)
       where TAdditionalProperties : class, new()
       where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<StringValueDetailListData<TAdditionalProperties, TTenantValue>>> GetStringValueDetailListAsync<TAdditionalProperties, TTenantValue>(Verisoft.Core.Contracts.FilteredRequest<StringValueDetailFilter> request)
    where TAdditionalProperties : class, new()
    where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> AddStringValueAsync<TAdditionalProperties, TTenantValue>(StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> RemoveStringValueAsync<TAdditionalProperties, TTenantValue>(string stringValue)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> UpdateStringValueAsync<TAdditionalProperties, TTenantValue>(string stringValue, StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new();

    Task<BusinessActionResult<StringValueDetailListData<TAdditionalProperties, TTenantValue>>> AddStringValuesInBulkAsync<TAdditionalProperties, TTenantValue>(StringValueBulkAddModel<TAdditionalProperties, TTenantValue> stringValueBulkAddModel, string codebookRecordTableName)
    where TAdditionalProperties : class, new()
    where TTenantValue : class, ITenantValue, new();

    public Task<BusinessActionResult<StringValueDetailValidatedModel<TAdditionalProperties, TTenantValue>>> ValidateAsync<TAdditionalProperties, TTenantValue>(string stringValue, bool? isGlobal, StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail, bool isCreationValidation)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new();
}