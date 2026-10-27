using Microsoft.EntityFrameworkCore;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class BlobRepository(IDemoApiDbContext unitOfWork) : BaseRepository<BlobEntity, int>(unitOfWork), IBlobRepository
{
    protected override DbSet<BlobEntity> GetDbSet()
    {
        return Context.Blob;
    }
}
