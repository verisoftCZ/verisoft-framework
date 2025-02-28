using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Authentication.Permissions;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Enums;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(AuthorizationResult))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ClientController(IClientService clientService)
{
    [HttpGet]
    [Route("{id}")]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    public async Task<IActionResult> GetClientAsync(int id)
    {
        var result = await clientService.GetClientAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientListData))]
    public async Task<ClientListData> GetClientListAsync([FromQuery] FilteredRequest<ClientFilter> request)
    {
        return await clientService.GetClientListAsync(request);
    }

    [HttpGet]
    [Route("history")]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientListData))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionResult<ClientListData>))]
    public async Task<BusinessActionResult<ClientListData>> GetClientHistoryListAsync([FromQuery] FilteredRequest<ClientHistoryFilter> request)
    {
        return await clientService.GetClientHistoryListAsync(request);
    }

    [HttpGet]
    [Route("export")]
    [HasPermission(DemoApiPermissions.ReadClient)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status301MovedPermanently, Type = typeof(RedirectResult))]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType, Type = typeof(JsonResult))]
    public async Task<IActionResult> GetClientsAsync([FromQuery] FilteredRequest<ClientFilter> request, [FromQuery] ExportType exportType)
    {
        var result = await clientService.GetClientsForExportAsync(request, exportType);

        if (result.IsSuccess && result.Data is ExportFileResult fileResult)
        {
            return new FileStreamResult(fileResult.FileStream, fileResult.FileType)
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<Client>))] // metoda zatim toto nevraci
    public async Task<IActionResult> AddClientAsync(ClientEditModel clientEditModel)
    {
        var result = await clientService.AddClientAsync(clientEditModel);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [HasPermission(DemoApiPermissions.UpdateClient)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<Client>))] // metoda zatim toto nevraci
    public async Task<IActionResult> UpdateClientsAsync(ClientEditModel clientEditModel, int id)
    {
        var result = await clientService.UpdateClientAsync(clientEditModel, id);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [HasPermission(DemoApiPermissions.DeleteClient)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    public async Task<IActionResult> DeleteClientAsync(int id)
    {
        var result = await clientService.RemoveClientAsync(id);
        return result.ToActionResult();
    }
}