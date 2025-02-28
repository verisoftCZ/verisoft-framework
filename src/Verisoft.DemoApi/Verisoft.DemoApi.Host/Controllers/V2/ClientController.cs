using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation.Extensions;
using Verisoft.Core.Validation.Services;
using Verisoft.Core.ExportStrategies.Enums;
using Verisoft.Core.ExportStrategies.ExportResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Host.Controllers.V2;

/// <summary>
/// Controller for managing clients.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(AuthorizationResult))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ClientController(IClientService clientService, IValidationService validationService)
{
    /// <summary>
    /// Get details of a specific client by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the client.</param>
    /// <returns>The client details.</returns>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [AllowAnonymous]
    public async Task<IActionResult> GetClientAsync(int id)
    {
        var result = await clientService.GetClientAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a filtered list of clients.
    /// </summary>
    /// <param name="request">Filtering criteria.</param>
    /// <returns>A list of clients matching the criteria.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientListData))]
    [AllowAnonymous]
    public async Task<ClientListData> GetClientListAsync([FromQuery] FilteredRequest<ClientFilter> request)
    {
        return await clientService.GetClientListAsync(request);
    }

    /// <summary>
    /// Add a new client.
    /// </summary>
    /// <param name="clientEditModel">Client details.</param>
    /// <returns>The created client.</returns>
    [HttpGet]
    [Route("export")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [AllowAnonymous]
    public async Task<IActionResult> GetClientsAsync([FromQuery] FilteredRequest<ClientFilter> request, ExportType exportType)
    {
        var result = await clientService.GetClientsForExportAsync(request, exportType);

        if (result.IsSuccess && result.Data is ExportFileResult fileResult)
        {
            return new FileStreamResult(fileResult.FileStream, fileResult.FileType)
            {
                FileDownloadName = fileResult.FileName,
            };
        }

        return result.ToActionResult();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<ClientValidatedObject>))]
    [AllowAnonymous]
    public async Task<IActionResult> AddClientAsync(ClientEditModel clientEditModel)
    {
        var validatedObject = await validationService.ValidateAsync<ClientValidatedObject, ClientEditModel>(clientEditModel);
        if (validatedObject.HasValidationErrors())
        {
            return validatedObject.ToActionResult();
        }

        var result = await clientService.AddClientAsync(clientEditModel);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<ClientValidatedObject>))]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateClientsAsync(ClientEditModel clientEditModel, int id)
    {
        var validatedObject = await validationService.ValidateAsync<ClientValidatedObject, ClientEditModel>(clientEditModel, id);
        if (validatedObject.HasValidationErrors())
        {
            return validatedObject.ToActionResult();
        }

        var result = await clientService.UpdateClientAsync(clientEditModel, id);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteClientAsync(int id)
    {
        var result = await clientService.RemoveClientAsync(id);
        return result.ToActionResult();
    }
}