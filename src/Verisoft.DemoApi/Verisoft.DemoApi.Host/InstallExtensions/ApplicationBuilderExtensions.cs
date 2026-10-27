using Verisoft.Core.AspNet;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Host.InstallExtensions;

public static class ApplicationBuilderExtensions
{
    public static void UseDemoApi(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        applicationBuilder.UseExceptionHandler();
        applicationBuilder.MigrateDatabase<IDemoApiDbContext>();
        applicationBuilder.SeedDemoData(configuration);
    }
}
