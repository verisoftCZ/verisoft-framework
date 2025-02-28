using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Enums;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Document;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(AuthorizationResult))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Verisoft.Core.Contracts.FailureDetail))]
public class DocumentController(IDocumentService documentService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFound))]
    public async Task<IActionResult> GetDocumentAsync(int id)
    {
        var result = await documentService.GetDocumentAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Document>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    public async Task<IEnumerable<Document>> GetDocumentsAsync([FromQuery] Verisoft.Core.Contracts.FilteredRequest<DocumentFilter> request)
    {
        return await documentService.GetDocumentsAsync(request);
    }

    [HttpGet]
    [Route("{id}/download")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    public async Task<IActionResult> DownloadDocumentAsync(int id)
    {
        return await documentService.GetDocumentContentAsync(id);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(UnprocessableEntity))]
    public async Task<IActionResult> AddDocumentAsync([FromForm] DocumentCreateModel documentEditModel, IFormFile document)
    {
        var result = await documentService.AddDocumentAsync(documentEditModel, document);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundObjectResult))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(UnprocessableEntity))]
    public async Task<IActionResult> UpdateClientsAsync([FromForm] DocumentUpdateModel clientEditModel, int id, IFormFile document)
    {
        var result = await documentService.UpdateDocumentAsync(id, clientEditModel, document);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoDelete},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundObjectResult))]
    public async Task<IActionResult> DeleteClientAsync(int id)
    {
        var result = await documentService.RemoveDocumentAsync(id);
        return result.ToActionResult();
    }
}