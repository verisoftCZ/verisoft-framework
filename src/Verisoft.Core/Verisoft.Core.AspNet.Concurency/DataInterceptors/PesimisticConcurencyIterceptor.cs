using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Data.EntityFramework.Atributes;

namespace Verisoft.Core.AspNet.Concurency.DataInterceptors;

[RegisterInterceptor]
public sealed class PesimisticConcurencyIterceptor : ISaveChangesInterceptor
{
    InterceptionResult<int> ISaveChangesInterceptor.SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateVersion(eventData.Context);
        return result;
    }

    ValueTask<InterceptionResult<int>> ISaveChangesInterceptor.SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
    {
        UpdateVersion(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void UpdateVersion(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is IPesimisticLockable lockable
                && entry.State == EntityState.Modified
                && entry.OriginalValues[nameof(IPesimisticLockable.LockedAt)] != null
                && !Equals(entry.OriginalValues[nameof(IPesimisticLockable.LockedBy)], GetUpdatedBy(entry.Entity)))
            {
                throw new DbUpdateConcurrencyException("Pesimistic");
            }
        }
    }

    private string GetUpdatedBy(object entity)
    {
        return entity.GetType().GetProperty(nameof(IBaseEntity<object>.UpdatedBy))?.GetValue(entity) as string;
    }
}
