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

namespace Verisoft.DemoApi.Host.Controllers.V2;

[AllowAnonymous]
[ApiController]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<AuthorizationResult>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class DocumentController(IDocumentService documentService) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<Document>(StatusCodes.Status200OK)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> GetDocumentAsync(int id)
    {
        var result = await documentService.GetDocumentAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<IEnumerable<Document>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<Document>?> GetDocumentsAsync([FromQuery] Verisoft.Core.Contracts.FilteredRequest<DocumentFilter> request)
    {
        return await documentService.GetDocumentsAsync(request);
    }

    [HttpGet]
    [Route("{id}/download")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    public async Task<IActionResult> DowloadDocumentAsync(int id)
    {
        return await documentService.GetDocumentContentAsync(id);
    }

    [HttpPost]
    [ProducesResponseType<Document>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UnprocessableEntity>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult?> AddDocumentAsync([FromForm] DocumentCreateModel documentEditModel, IFormFile document)
    {
        var result = await documentService.AddDocumentAsync(documentEditModel, document);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<Document>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<NotFoundObjectResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UnprocessableEntity>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult?> UpdateClientsAsync([FromForm] DocumentUpdateModel clientEditModel, int id, IFormFile document)
    {
        var result = await documentService.UpdateDocumentAsync(id, clientEditModel, document);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoDelete},{Roles.Admin}")]
    [ProducesResponseType<Document>(StatusCodes.Status200OK)]
    [ProducesResponseType<NotFoundObjectResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> DeleteClientAsync(int id)
    {
        var result = await documentService.RemoveDocumentAsync(id);
        return result.ToActionResult();
    }
}
