using Verisoft.CodebookApi.Contracts.Models.Language;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface ILanguageService
{
    Task<BusinessActionResult<Language>> AddLanguageAsync(LanguageEditModel languageEditModel);

    Task<BusinessActionResult<Language>> GetLanguageAsync(int id);

    Task<IEnumerable<Language>> GetLanguagesAsync();

    Task<BusinessActionResult<Language>> RemoveLanguageAsync(int id);

    Task<BusinessActionResult<Language>> UpdateLanguageAsync(LanguageEditModel languageEditModel, int id);
}