using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Codebook;
using Verisoft.Core.Contracts;
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
public class CodebookController(ICodebookService codebookService, IValidationService validationService) : Controller
{
    private readonly ICodebookService codebookService = codebookService ?? throw new ArgumentNullException(nameof(codebookService));
    private readonly IValidationService validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Codebook))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<CodebookValidatedModel>))]
    public async Task<IActionResult> AddCodebookAsync(CodebookEditModel codebookEditModel)
    {
        var validationResult = await validationService.ValidateAsync<CodebookValidatedModel, CodebookEditModel>(codebookEditModel);

        if (validationResult.HasValidationErrors())
        {
            return validationResult.ToActionResult();
        }

        var result = await codebookService.AddCodebookAsync(codebookEditModel);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Codebook))]
    public async Task<IActionResult> GetCodebookAsync(int id)
    {
        var result = await codebookService.GetCodebookAsync(id);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Codebook))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<CodebookValidatedModel>))]
    public async Task<IActionResult> UpdateCodebookAsync(int id, CodebookEditModel codebookEditModel)
    {
        var validationResult = await validationService.ValidateAsync<CodebookValidatedModel, CodebookEditModel>(codebookEditModel, id);

        if (validationResult.HasValidationErrors())
        {
            return validationResult.ToActionResult();
        }

        var result = await codebookService.UpdateCodebookAsync(id, codebookEditModel);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Codebook))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    public async Task<IActionResult> RemoveCodebookAsync(int id)
    {
        var result = await codebookService.RemoveCodebookAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CodebookListData))]
    public async Task<IActionResult> GetCodebooksAsync([FromQuery] FilteredRequest<CodebookFilter> request)
    {
        var result = await codebookService.GetCodebooksAsync(request);
        return result.ToActionResult();
    }
}
