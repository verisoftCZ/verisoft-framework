using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Data.EntityFramework.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public abstract class BaseRepository<TEntity, TKey>(
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    ILogger<BaseRepository<TEntity, TKey>> logger,
    bool throwIfAuditFails = false,
    IEnumerable<string> ignoredProperties = null) : EntityFrameworkRepositoryBase<TEntity, TKey>(unitOfWork, userContext, logger, throwIfAuditFails, ignoredProperties)
    where TEntity : class, IBaseEntity<TKey>
{
    protected IDemoApiDbContext Context => (IDemoApiDbContext)UnitOfWork;
}
