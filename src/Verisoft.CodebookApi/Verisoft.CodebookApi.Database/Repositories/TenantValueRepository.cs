using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Database.Context;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class TenantValueRepository(ICodebookApiDbContext unitOfWork, IUserContext userContext, ILogger<TenantValueRepository> logger) : EntityFrameworkRepositoryBase<TenantValueEntity, int>(unitOfWork, userContext, logger), ITenantValueRepository
{
    protected ICodebookApiDbContext Context => (ICodebookApiDbContext)UnitOfWork;

    protected override DbSet<TenantValueEntity> GetDbSet()
    {
        return Context.TenantValue;
    }
}
