using Verisoft.CodebookApi.Database.Context;
using Verisoft.Core.AspNet;

namespace Verisoft.CodebookApi.Host.InstallExtensions;

public static class ApplicationBuilderExtensions
{
    public static void UseCodebookApi(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseVerisoftCoreAspNet();
        applicationBuilder.MigrateDatabase<ICodebookApiDbContext>();
    }
}
