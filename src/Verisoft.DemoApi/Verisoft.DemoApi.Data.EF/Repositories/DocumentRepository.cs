using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class DocumentRepository(IDemoApiDbContext unitOfWork, IUserContext userContext, ILogger<DocumentRepository> logger) : BaseRepository<DocumentEntity, int>(unitOfWork, userContext, logger), IDocumentRepository
{
    public Task<DocumentEntity> GetDocumentWithBlobContent(int id)
    {
        return Context.Document
            .Include(x => x.Blob)
            .FirstAsync(x => x.Id == id);
    }

    protected override DbSet<DocumentEntity> GetDbSet()
    {
        return Context.Document;
    }
}
