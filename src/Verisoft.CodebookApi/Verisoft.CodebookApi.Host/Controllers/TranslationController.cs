using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Models.Translation;
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
public class TranslationController(ITranslationService translationService, IValidationService validationService) : Controller
{
    private readonly ITranslationService translationService = translationService ?? throw new ArgumentNullException(nameof(translationService));
    private readonly IValidationService validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

    [HttpGet]
    [Route("{tableName}/{stringValue}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StringValueTranslations))]
    public async Task<StringValueTranslations> GetStringValueTranslationsAsync(string tableName, string stringValue)
    {
        return await translationService.GetStringValueTranslationsAsync(tableName, stringValue);
    }

    [HttpPut]
    [Route("{tableName}/{stringValue}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StringValueTranslations))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<StringValueTranslationsValidatedModel>))]
    public async Task<IActionResult> UpdateStringValueTranslationsAsync(string tableName, string stringValue, StringValueTranslations translations)
    {
        var validationResult = await validationService.ValidateAsync<StringValueTranslationsValidatedModel, StringValueTranslations>(translations);
        if (validationResult.HasValidationErrors())
        {
            return validationResult.ToActionResult();
        }

        var result = await translationService.UpdateStringValueTranslationsAsync(tableName, stringValue, translations);
        return result.ToActionResult();
    }
}
