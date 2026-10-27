using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Validation.Services;
using Verisoft.DemoApi.Application.Controllers;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
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
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<AuthorizationResult>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ClientController(IClientService clientService, IClientExportService clientExportService, IValidationService validationService)
    : CrudController<Client, ClientEditModel, ClientEditModel, ClientValidatedObject, ClientListItem, ClientFilter, ClientEntity, int>(clientService, clientExportService, validationService)
{
    /// <summary>
    /// Get details of a specific client by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the client.</param>
    /// <returns>The client details.</returns>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    public async Task<IActionResult?> GetClientAsync(int id) => await GetAsync(id);

    /// <summary>
    /// Get a filtered list of clients.
    /// </summary>
    /// <param name="request">Filtering criteria.</param>
    /// <returns>A list of clients matching the criteria.</returns>
    [HttpGet]
    [ProducesResponseType<PagedData<ClientListItem>>(StatusCodes.Status200OK)]
    public async Task<PagedData<ClientListItem>> GetClientListAsync([FromQuery] FilteredRequest<ClientFilter> request) => await GetListAsync(request);

    /// <summary>
    /// Get projection list of clients
    /// </summary>
    /// <param name="request"></param>
    /// <returns>list of projections</returns>
    [HttpGet("GetProjectedExample")]
    [ProducesResponseType<ClientProjectionExample>(StatusCodes.Status200OK)]
    public async Task<PagedData<ClientProjectionExample>> GetProjectedExampleAsync([FromQuery] FilteredRequest<ClientFilter> request) => await clientService.GetClientsProjectedAsync(request);

    /// <summary>
    /// Get projection of a specific client by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>projection of client</returns>
    [HttpGet("GetOneProjectedExample")]
    [ProducesResponseType<ClientProjectionExample>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOneProjectedExampleAsync([FromQuery] int id)
    {
        var result = await clientService.GetOneClientProjectedAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Add a new client.
    /// </summary>
    /// <param name="request">ExportDataRequest</param>
    /// <returns>The created client.</returns>
    [HttpPost]
    [Route("export")]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    public async Task<IActionResult?> GetClientsAsync(ExportDataRequest<ClientFilter> request) => await ExportDataAsync(request);

    [HttpPost]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<BusinessActionErrors<ClientValidatedObject>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult?> AddClientAsync(ClientEditModel clientEditModel) => await AddAsync(clientEditModel);

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BusinessActionErrors<ClientValidatedObject>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult?> UpdateClientsAsync(ClientEditModel clientEditModel, int id) => await UpdateAsync(id, clientEditModel);

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType<Client>(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> DeleteClientAsync(int id) => await DeleteAsync(id);

    [HttpPost]
    [Route("AddBulk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddClientsBulkAsync(ClientEditModel[] clientEditModels) => await AddBulkAsync(clientEditModels);

    [HttpPost]
    [Route("DeleteBulk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<BusinessActionErrors>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteClientsBulkAsync(List<int> ids) => await DeleteBulkAsync(ids);
}