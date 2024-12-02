using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class BlobRepository(IDemoApiDbContext unitOfWork, IUserContext userContext, ILogger<BlobRepository> logger) : BaseRepository<BlobEntity, int>(unitOfWork, userContext, logger), IBlobRepository
{
    protected override DbSet<BlobEntity> GetDbSet()
    {
        return Context.Blob;
    }
}
