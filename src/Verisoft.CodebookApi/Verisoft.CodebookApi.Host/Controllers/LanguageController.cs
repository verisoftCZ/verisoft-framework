using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Models.Language;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation.Extensions;
using Verisoft.Core.Validation.Services;

namespace Verisoft.CodebookApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class LanguageController(ILanguageService languageService, IValidationService validationService) : Controller
{
    private readonly ILanguageService languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
    private readonly IValidationService validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Language))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Language))]
    public async Task<IActionResult> GetLanguageAsync(int id)
    {
        var result = await languageService.GetLanguageAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Language>))]
    public async Task<IEnumerable<Language>> GetLanguagesAsync()
    {
        return await languageService.GetLanguagesAsync();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Language))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Language))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    public async Task<IActionResult> RemoveLanguageAsync(int id)
    {
        var result = await languageService.RemoveLanguageAsync(id);
        return result.ToActionResult();
    }
}
