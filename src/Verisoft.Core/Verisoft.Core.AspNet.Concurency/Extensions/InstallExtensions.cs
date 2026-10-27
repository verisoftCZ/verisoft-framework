using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.AspNet.Concurency.Exceptions;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.AspNet.Concurency.Extensions;

public static class InstallExtensions
{
    public static IServiceCollection AddConcurency<TDbContext>(this IServiceCollection services)
    where TDbContext : IUnitOfWork
    {
        services.AddScoped<ILockingManager, LockingManager<TDbContext>>();
        services.AddExceptionHandler<ConcurrencyExceptionHandler>();

        return services;
    }

    public static WebApplication UseConcurency(this WebApplication app)
    {
        var entityTypes = GetEntities();
        foreach (var entityType in entityTypes)
        {
            app.MapPost($$"""{{entityType.Name}}/{id}/lock""", async ([FromRoute] int id, [FromServices] ILockingManager lockingManager) =>
            {
                MethodInfo? genericMethod = typeof(ILockingManager).GetMethod(nameof(lockingManager.Lock));
                if (genericMethod == null)
                {
                    throw new InvalidOperationException("Method 'Lock' not found on ILockingManager.");
                }

                MethodInfo? constructedMethod = genericMethod?.MakeGenericMethod(entityType);
                if (constructedMethod == null)
                {
                    throw new InvalidOperationException($"Could not construct generic method for {entityType.Name}.");
                }

                await (Task)constructedMethod.Invoke(lockingManager, [id])!;
            }).WithTags(entityType.Name)
              .RequireAuthorization()
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
              .Produces(StatusCodes.Status422UnprocessableEntity, typeof(UnprocessableEntity))
              .Produces(StatusCodes.Status403Forbidden, typeof(ForbidResult));

            app.MapDelete($$"""{{entityType.Name}}/{id}/lock""", async ([FromRoute] int id, [FromServices] ILockingManager lockingManager) =>
            {
                MethodInfo? genericMethod = typeof(ILockingManager).GetMethod(nameof(lockingManager.Unlock));
                if (genericMethod == null)
                {
                    throw new InvalidOperationException("Method 'Lock' not found on ILockingManager.");
                }

                MethodInfo? constructedMethod =
                genericMethod?.MakeGenericMethod(entityType);
                if (constructedMethod == null)
                {
                    throw new InvalidOperationException($"Could not construct generic method for {entityType.Name}.");
                }

                await (Task)constructedMethod.Invoke(lockingManager, [id])!;
            }).WithTags(entityType.Name)
              .RequireAuthorization()
              .Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
              .Produces(StatusCodes.Status422UnprocessableEntity, typeof(UnprocessableEntity))
              .Produces(StatusCodes.Status403Forbidden, typeof(ForbidResult));
        }

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
