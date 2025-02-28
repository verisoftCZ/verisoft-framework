using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class DocumentRepository(IDemoApiDbContext unitOfWork, IUserContext userContext) : BaseRepository<DocumentEntity, int>(unitOfWork, userContext), IDocumentRepository
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
