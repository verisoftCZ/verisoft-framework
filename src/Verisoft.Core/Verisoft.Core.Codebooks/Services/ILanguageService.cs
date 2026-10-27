using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

public interface ILanguageService
{
    Task<BusinessActionResult<Language>> AddLanguageAsync(LanguageEditModel languageEditModel);

    Task<BusinessActionResult<Language>> GetLanguageAsync(int id);

    Task<IEnumerable<Language>> GetLanguagesAsync();

    Task<BusinessActionResult<Language>> RemoveLanguageAsync(int id);

    Task<BusinessActionResult<Language>> UpdateLanguageAsync(LanguageEditModel languageEditModel, int id);
}