using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class BlobRepository(IDemoApiDbContext unitOfWork, IUserContext userContext) : BaseRepository<BlobEntity, int>(unitOfWork, userContext), IBlobRepository
{
    protected override DbSet<BlobEntity> GetDbSet()
    {
        return Context.Blob;
    }
}
