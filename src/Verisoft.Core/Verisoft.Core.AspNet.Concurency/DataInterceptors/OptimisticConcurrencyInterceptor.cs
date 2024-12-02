using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Data.EntityFramework.Atributes;

namespace Verisoft.Core.AspNet.Concurency.DataInterceptors
{
    [RegisterInterceptor]
    public sealed class OptimisticConcurrencyInterceptor : ISaveChangesInterceptor
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

        private static void UpdateVersion(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is IOptimisticLockable lockable && entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IOptimisticLockable.Version)).OriginalValue = lockable.Version;
                }
            }
        }
    }
}
