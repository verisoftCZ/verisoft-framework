using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.ExportResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;
using Verisoft.DemoApi.Contracts.Models.CodebookParent;

namespace Verisoft.DemoApi.Host.Controllers.V2;

[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<AuthorizationResult>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public sealed class CodebookParentController(
    ICodebookParentService service,
    ICodebookParentExportService exportService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetListAsync([FromQuery] FilteredRequest<CodebookParentFilter> request)
    {
        var result = await service.GetPagedAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<CodebookParentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var result = await service.GetByIdAsync(id);
        return result.ToActionResult();
    }

    [HttpPost]
    [ProducesResponseType<CodebookParentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UnprocessableEntity>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateAsync([FromBody] CodebookParentEditModel model)
    {
        var result = await service.CreateAsync(model);
        return result.ToActionResult();
    }

    [HttpGet("exists-for-client/{clientId:int}")]
    [ProducesResponseType<CodebookParentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UnprocessableEntity>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ExistsForClientAsync(int clientId)
    {
        var result = await service.ExistsForClientAsync(clientId);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("export")]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    public async Task<IActionResult?> GetClientsAsync(ExportDataRequest<CodebookParentFilter> request)
    {
        ExportFileResult? exportResult = null;
        try
        {
            exportResult = await exportService.ExportDataAsync(request);
            return File(exportResult.FileStream!, exportResult.FileType!, exportResult.FileName);
        }
        catch (Exception)
        {
            if (exportResult is not null)
            {
                await exportResult.FileStream!.DisposeAsync();
            }

            throw;
        }
    }
}
