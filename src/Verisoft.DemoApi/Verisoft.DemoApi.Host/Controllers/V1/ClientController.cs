using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Authentication.Permissions;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Export.Enums;
using Verisoft.Core.Export.ExportResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Enums;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<AuthorizationResult>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ClientController(IClientService clientService, IClientExportService clientExportService)
{
    [HttpGet]
    [Route("{id}")]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> GetClientAsync(int id)
    {
        var result = await clientService.GetClientAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType<PagedData<ClientListItem>>(StatusCodes.Status200OK)]
    public async Task<PagedData<ClientListItem>> GetClientListAsync([FromQuery] FilteredRequest<ClientFilter> request)
    {
        return await clientService.GetClientListAsync(request);
    }

    [HttpGet]
    [Route("history")]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType<ClientListData>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionResult<ClientListData>>(StatusCodes.Status400BadRequest)]
    public async Task<BusinessActionResult<ClientListData>> GetClientHistoryListAsync([FromQuery] FilteredRequest<ClientHistoryFilter> request)
    {
        return await clientService.GetClientHistoryListAsync(request);
    }

    [HttpGet]
    [Route("export")]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType<FileStreamResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<RedirectResult>(StatusCodes.Status301MovedPermanently)]
    [ProducesResponseType<JsonResult>(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<IActionResult?> GetClientsAsync([FromQuery] ExportDataRequest<ClientFilter> request)
    {
        var result = await clientExportService.GetClientsForExportAsync(request);

        if (result.IsSuccess && result.Data is ExportFileResult fileResult)
        {
            return new FileStreamResult(fileResult.FileStream!, fileResult.FileType!)
            {
                FileDownloadName = fileResult.FileName,
            };
        }

        var customActionResults = new Dictionary<string, IActionResult>
        {
            [StatusCodes.Status301MovedPermanently.ToString()] = result.Data is ExportJobInitiatedResult jobResult
                ? new RedirectResult($"/status/{jobResult.JobId}")
                : new BadRequestResult(),
            [StatusCodes.Status415UnsupportedMediaType.ToString()] = new JsonResult(new
            {
                message = "The requested export type is not supported.",
                supportedTypes = Enum.GetNames<ExportType>(),
            })
            {
                StatusCode = StatusCodes.Status415UnsupportedMediaType,
            },
        };

        return result.ToActionResult(customActionResults);
    }

    [HttpPost]
    [HasPermission(DemoApiPermissions.UpdateClient)]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<BusinessActionErrors<Client>>(StatusCodes.Status422UnprocessableEntity)] // metoda zatim toto nevraci
    public async Task<IActionResult?> AddClientAsync(ClientEditModel clientEditModel)
    {
        var result = await clientService.AddClientAsync(clientEditModel);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [HasPermission(DemoApiPermissions.UpdateClient)]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BusinessActionErrors<Client>>(StatusCodes.Status422UnprocessableEntity)] // metoda zatim toto nevraci
    public async Task<IActionResult?> UpdateClientsAsync(ClientEditModel clientEditModel, int id)
    {
        var result = await clientService.UpdateClientAsync(clientEditModel, id);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [HasPermission(DemoApiPermissions.DeleteClient)]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> DeleteClientAsync(int id)
    {
        var result = await clientService.RemoveClientAsync(id);
        return result.ToActionResult();
    }
}