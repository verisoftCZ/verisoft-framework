using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Services;
using Verisoft.Core.Contracts;
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
public class CodebookController(ICodebookService codebookService, IValidationService validationService) : Controller
{
    private readonly ICodebookService codebookService = codebookService ?? throw new ArgumentNullException(nameof(codebookService));
    private readonly IValidationService validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

    [HttpPost]
    [ProducesResponseType<Codebook>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors<CodebookValidatedModel>>(StatusCodes.Status422UnprocessableEntity)]
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
    [ProducesResponseType<Codebook>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCodebookAsync(int id)
    {
        var result = await codebookService.GetCodebookAsync(id);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType<Codebook>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BusinessActionErrors<CodebookValidatedModel>>(StatusCodes.Status422UnprocessableEntity)]
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
    [ProducesResponseType<Codebook>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveCodebookAsync(int id)
    {
        var result = await codebookService.RemoveCodebookAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [ProducesResponseType<CodebookListData>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCodebooksAsync([FromQuery] FilteredRequest<CodebookFilter> request)
    {
        var result = await codebookService.GetCodebooksAsync(request);
        return result.ToActionResult();
    }
}
