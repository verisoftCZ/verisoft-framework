using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.AspNet.Concurency.Middleware;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.AspNet.Concurency.Extensions;

public static class InstallExtensions
{
    public static IServiceCollection AddConcurency<TDbContext>(this IServiceCollection services)
    where TDbContext : IUnitOfWork
    {
        services.AddScoped<ILockingManager, LockingManager<TDbContext>>();

        return services;
    }

    public static WebApplication UseConcurency(this WebApplication app)
    {
        var entityTypes = GetEntities();
        foreach (var entityType in entityTypes)
        {
            app.MapPost($$"""{{entityType.Name}}/{id}/lock""", async ([FromRoute] int id, [FromServices] ILockingManager lockingManager) =>
            {
                MethodInfo genericMethod = typeof(ILockingManager).GetMethod(nameof(lockingManager.Lock));
                MethodInfo constructedMethod = genericMethod.MakeGenericMethod(entityType);

                await (Task)constructedMethod.Invoke(lockingManager, [id]);
            }).WithTags(entityType.Name)
              .RequireAuthorization()
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
              .Produces(StatusCodes.Status422UnprocessableEntity, typeof(UnprocessableEntity))
              .Produces(StatusCodes.Status403Forbidden, typeof(ForbidResult));

            app.MapDelete($$"""{{entityType.Name}}/{id}/lock""", async ([FromRoute] int id, [FromServices] ILockingManager lockingManager) =>
            {
                MethodInfo genericMethod = typeof(ILockingManager).GetMethod(nameof(lockingManager.Unlock));
                MethodInfo constructedMethod = genericMethod.MakeGenericMethod(entityType);

                await (Task)constructedMethod.Invoke(lockingManager, [id]);
            }).WithTags(entityType.Name)
              .RequireAuthorization()
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
              .Produces(StatusCodes.Status422UnprocessableEntity, typeof(UnprocessableEntity))
              .Produces(StatusCodes.Status403Forbidden, typeof(ForbidResult));
        }

        app.UseMiddleware<ConncurencyExceptionHandlingMiddleware>();

        return app;
    }

    private static List<Type> GetEntities()
    {
        var type = typeof(IPesimisticLockable);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetExportedTypes())
            .Where(x => x.IsClass)
            .Where(type.IsAssignableFrom)
            .ToList();
    }
}
