using Verisoft.CodebookApi.Contracts.Models.Translation;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface ITranslationService
{
    Task<StringValueTranslations> GetStringValueTranslationsAsync(string tableName, string stringValue);

    Task<BusinessActionResult<StringValueTranslations>> UpdateStringValueTranslationsAsync(string tableName, string stringValue, StringValueTranslations translations);
}