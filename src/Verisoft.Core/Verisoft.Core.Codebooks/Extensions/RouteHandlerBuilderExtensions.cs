using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Codebooks.Authorization;

namespace Verisoft.Core.Codebooks.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, params string[] permissions)
        => builder.AddEndpointFilter(async (context, next) =>
            {
                var httpContext = context.HttpContext;
                if (httpContext.User?.Identity?.IsAuthenticated != true)
                {
                    return TypedResults.Unauthorized();
                }

                var permissionChecker = httpContext.RequestServices.GetRequiredService<ICodebookAuthorizationChecker>();

                var hasPermission = await permissionChecker.HasPermissionAsync(permissions);
                if (!hasPermission.HasPermission)
                {
                    return Results.Json(
                        hasPermission.Error,
                        statusCode: StatusCodes.Status403Forbidden);
                }

                return await next(context);
            });
}
