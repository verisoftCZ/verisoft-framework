using Verisoft.Core.AspNet;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Host.InstallExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDemoApi(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseVerisoftCoreAspNet();
            applicationBuilder.MigrateDatabase<IDemoApiDbContext>();
        }
    }
}
