using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Common.Logging;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.AspNet;

public static class ApplicationBuilderExtensions
{
    public static void UseVerisoftAvailabilityTracking(this IApplicationBuilder app)
    {
        var tracker = app.ApplicationServices.GetService<IBusinessTracker>();
        tracker?.StartSelfAvailabilityTest(CancellationToken.None);
    }

    public static void MigrateDatabase<TDBContext>(this IApplicationBuilder app)
        where TDBContext : IMigrable
    {
        var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
        if (scopeFactory == null)
        {
            return;
        }

        using var scope = scopeFactory.CreateScope();
        scope.ServiceProvider.GetRequiredService<TDBContext>().Migrate();
    }
}
