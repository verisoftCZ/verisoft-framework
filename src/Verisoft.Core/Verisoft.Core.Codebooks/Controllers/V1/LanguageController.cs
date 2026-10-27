using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Services;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation.Extensions;
using Verisoft.Core.Validation.Services;

namespace Verisoft.Core.Codebooks.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[Authorize]
[ProducesResponseType<BadRequestResult>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class LanguageController(ILanguageService languageService, IValidationService validationService) : Controller
{
    private readonly ILanguageService languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
    private readonly IValidationService validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

    [HttpPost]
    [ProducesResponseType<Language>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddLanguageAsync(LanguageEditModel languageEditModel)
    {
        var validationResult = await validationService.ValidateAsync<LanguageValidatedModel, LanguageEditModel>(languageEditModel);

        if (validationResult.HasValidationErrors())
        {
            return validationResult.ToActionResult();
        }

        var result = await languageService.AddLanguageAsync(languageEditModel);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType<Language>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLanguageAsync(int id)
    {
        var result = await languageService.GetLanguageAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<Language>>(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Language>> GetLanguagesAsync()
    {
        return await languageService.GetLanguagesAsync();
    }

    [HttpPut]
    [ProducesResponseType<Language>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLanguageAsync(LanguageEditModel languageEditModel, int id)
    {
        var validationResult = await validationService.ValidateAsync<LanguageValidatedModel, LanguageEditModel>(languageEditModel, id);

        if (validationResult.HasValidationErrors())
        {
            return validationResult.ToActionResult();
        }

        var result = await languageService.UpdateLanguageAsync(languageEditModel, id);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType<Language>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveLanguageAsync(int id)
    {
        var result = await languageService.RemoveLanguageAsync(id);
        return result.ToActionResult();
    }
}
