using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Verisoft.Core.Common.Store;

public interface IUnitOfWork
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    void Commit();

    Task CommitAsync(CancellationToken cancellationToken = default);

    void Rollback();
}
