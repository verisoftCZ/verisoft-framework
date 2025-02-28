using System;
using Microsoft.EntityFrameworkCore;

namespace Verisoft.Core.Data.EntityFramework.Extensions;

public static class DbContextExtensions
{
    public static void RollbackChanges(this DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Detached && entry.State != EntityState.Unchanged)
            {
                entry.State = EntityState.Unchanged;
            }
        }
    }
}
