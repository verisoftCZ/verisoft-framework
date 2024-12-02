using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Document;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface IDocumentService
{
    Task<BusinessActionResult<Document>> GetDocumentAsync(int id);

    Task<IEnumerable<Document>> GetDocumentsAsync(FilteredRequest<DocumentFilter> request);

    Task<FileContentResult> GetDocumentContentAsync(int id);

    Task<BusinessActionResult<Document>> AddDocumentAsync(DocumentCreateModel documentEditModel, IFormFile document);

    Task<BusinessActionResult<Document>> UpdateDocumentAsync(int id, DocumentUpdateModel updateModel, IFormFile documentFile);

    Task<BusinessActionResult<Document>> RemoveDocumentAsync(int id);
}
