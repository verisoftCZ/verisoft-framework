using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

public interface IStringValueService<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
{
    Task<BusinessActionResult<StringValueDetail<TCodebookRecordType>>> GetStringValueDetailAsync(string stringValue);

    Task<BusinessActionResult<StringValueDetailListData<TCodebookRecordType>>> GetStringValueDetailListAsync(FilteredRequest<StringValueDetailFilter> request);

    Task<BusinessActionResult<StringValueDetail<TCodebookRecordType>>> AddStringValueAsync(StringValueDetailCreateModel<TCodebookRecordType> stringValueDetail);

    Task<BusinessActionResult> RemoveStringValueAsync(string stringValue);

    Task<BusinessActionResult<StringValueDetail<TCodebookRecordType>>> UpdateStringValueAsync(string stringValue, StringValueDetailEditModel<TCodebookRecordType> stringValueDetail);

    Task<BusinessActionResult<StringValueDetailValidatedModel<TCodebookRecordType>>> ValidateAsync(string stringValue, StringValueDetailEditModel<TCodebookRecordType> stringValueDetail, bool isCreationValidation);

    Task<BusinessActionResult<StringValueTranslations>> UpsertTranslationAsync(string stringValue, string languageCode, string value);

    Task<BusinessActionResult<StringValueTranslations>> RemoveTranslationAsync(string stringValue, string languageCode);
}