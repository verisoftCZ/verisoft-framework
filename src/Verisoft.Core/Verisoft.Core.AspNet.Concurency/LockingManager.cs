using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.AspNet.Concurency;

internal class LockingManager<TContext>(TContext context, IUserContext userContext)
    : ILockingManager
    where TContext : IUnitOfWork
{
    public async Task Lock<TEntity>(int id)
        where TEntity : class, IPesimisticLockable, IEntity
    {
        var entity = await GetEntity<TEntity>(id);
        entity.LockedAt = DateTime.Now;
        entity.LockedBy = userContext.User.Identity.Name;
        context.Commit();
    }

    public async Task Unlock<TEntity>(int id)
       where TEntity : class, IPesimisticLockable, IEntity
    {
        var entity = await GetEntity<TEntity>(id);
        entity.LockedAt = null;
        entity.LockedBy = null;
        context.Commit();
    }

    private async Task<IPesimisticLockable> GetEntity<TEntity>(int id)
        where TEntity : class, IPesimisticLockable, IEntity
    {
        return await GetDbContext().Set<TEntity>().FirstAsync(x => (int)x.Id == id);
    }

    private DbContext GetDbContext()
    {
        return context as DbContext ?? throw new ArgumentException();
    }
}
