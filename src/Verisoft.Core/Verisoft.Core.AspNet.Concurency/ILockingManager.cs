using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.AspNet.Concurency;

public interface ILockingManager
{
    Task Lock<TEntity>(int id)
       where TEntity : class, IPesimisticLockable, IEntity;

    Task Unlock<TEntity>(int id)
       where TEntity : class, IPesimisticLockable, IEntity;
}
