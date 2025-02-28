using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Document;

namespace Verisoft.DemoApi.Application.Services;

public class DocumentService(
    IDocumentRepository documentRepository,
    ITypeMapper typeMapper,
    IIdentityService identityService,
    IClientRepository clientRepository)
    : IDocumentService
{
    public async Task<BusinessActionResult<Document>> GetDocumentAsync(int id)
    {
        var documentEntity = await documentRepository.GetAsync(id);
        if (documentEntity is null)
        {
            return ErrorFactory.NotFound<DocumentEntity>(nameof(DocumentEntity.Id), id);
        }

        return typeMapper.Map<Document>(documentEntity);
    }

    public async Task<FileContentResult> GetDocumentContentAsync(int id)
    {
        var document = await documentRepository.GetDocumentWithBlobContent(id);
        return new FileContentResult(document.Blob.Content, document.ContentType)
        {
            FileDownloadName = document.Name,
        };
    }

    public async Task<IEnumerable<Document>> GetDocumentsAsync(FilteredRequest<DocumentFilter> request)
    {
        var documentSpecification = typeMapper.Map<DocumentSpecification>(request.Filter);
        var filter = documentSpecification?.SatisfiedBy();
        var items = await documentRepository.GetPagedSortAsync<DocumentSort>(
            request.Offset,
            request.Limit,
            filter,
            request.Sort);
        return typeMapper.Map<Document>(items).ToList();
    }

    public async Task<BusinessActionResult<Document>> AddDocumentAsync(DocumentCreateModel documentEditModel, IFormFile document)
    {
        if (documentEditModel is null)
        {
            return ErrorFactory.NullInput(nameof(documentEditModel));
        }

        if (document is null)
        {
            return ErrorFactory.NullInput(nameof(document));
        }

        bool clientExist = await clientRepository.ExistsAsync(documentEditModel.ClientId);
        if (!clientExist)
        {
            return ErrorFactory.NotFound<ClientEntity>(nameof(ClientEntity.Id), documentEditModel.ClientId);
        }

        var documentEntity = typeMapper.Map<DocumentEntity>(documentEditModel);
        documentEntity.Blob = await GetBlobEntityFromDocAsync(document, null);
        documentEntity.UserId = (await identityService.GetCurrentUser()).Id;
        documentEntity.Name = document.FileName;
        documentEntity.ContentType = document.ContentType;
        await documentRepository.AddAsync(documentEntity);
        documentEntity.Blob.CreatedBy = documentEntity.CreatedBy;
        documentEntity.Blob.CreatedAt = documentEntity.CreatedAt;
        documentRepository.UnitOfWork.Commit();
        return typeMapper.Map<Document>(documentEntity);
    }

    public async Task<BusinessActionResult<Document>> UpdateDocumentAsync(int id, DocumentUpdateModel updateModel, IFormFile documentFile)
    {
        var documentEntity = await documentRepository.GetDocumentWithBlobContent(id);
        if (documentEntity is null)
        {
            return ErrorFactory.NotFound<DocumentEntity>(nameof(DocumentEntity.Id), id);
        }

        if (updateModel != null)
        {
            documentEntity.Description = updateModel.Description;
        }

        if (documentFile != null)
        {
            documentEntity.Blob = await GetBlobEntityFromDocAsync(documentFile, documentEntity.Blob);
            documentEntity.UserId = (await identityService.GetCurrentUser()).Id;
            documentEntity.Name = documentFile.FileName;
            documentEntity.ContentType = documentFile.ContentType;
            documentRepository.Update(documentEntity);
            documentEntity.Blob.CreatedBy = documentEntity.UpdatedBy;
            documentEntity.Blob.CreatedAt = documentEntity.UpdatedAt.Value;
        }

        documentRepository.UnitOfWork.Commit();
        return typeMapper.Map<Document>(documentEntity);
    }

    public async Task<BusinessActionResult<Document>> RemoveDocumentAsync(int id)
    {
        var documentEntity = await documentRepository.GetAsync(id);

        if (documentEntity is null)
        {
            return ErrorFactory.NotFound<DocumentEntity>(nameof(DocumentEntity.Id), id);
        }

        documentRepository.Remove(documentEntity);
        documentRepository.UnitOfWork.Commit();

        return typeMapper.Map<Document>(documentEntity);
    }

    private static async Task<BlobEntity> GetBlobEntityFromDocAsync(IFormFile document, BlobEntity originalBlobEntity)
    {
        if (originalBlobEntity is null)
        {
            using var stream = new MemoryStream();
            await document.CopyToAsync(stream);
            var blobEntity = new BlobEntity
            {
                Content = stream.ToArray(),
            };

            return blobEntity;
        }

        using (var stream = new MemoryStream())
        {
            await document.CopyToAsync(stream);
            originalBlobEntity.Content = stream.ToArray();
        }

        return originalBlobEntity;
    }
}