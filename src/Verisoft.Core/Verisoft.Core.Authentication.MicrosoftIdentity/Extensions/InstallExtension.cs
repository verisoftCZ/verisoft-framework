using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Authentication.MicrosoftIdentity.Builders;
using Verisoft.Core.Authentication.MicrosoftIdentity.Configuration;
using Verisoft.Core.Authentication.MicrosoftIdentity.Data;
using Verisoft.Core.Authentication.MicrosoftIdentity.Models;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.Authentication.MicrosoftIdentity.Extensions;

public static class InstallExtension
{
    public static IServiceCollection AddVerisoftIdentity(this IServiceCollection services, Action<IdentityConfigurationBuilder<IdentityUser>> configuration = null)
    {
        AddVerisoftIdentity<IdentityUser, IdentityEntityFrameworkDbContext<IdentityUser>>(services, configuration);

        return services;
    }

    public static IServiceCollection AddVerisoftIdentity<TUser, TIdetityDbContext>(this IServiceCollection services, Action<IdentityConfigurationBuilder<TUser>> configuration = null)
        where TUser : IdentityUser
        where TIdetityDbContext : IdentityDbContext<TUser>, IMigrable
    {
        var configBuilder = new IdentityConfigurationBuilder<TUser>();
        if (configuration != null)
        {
            configuration(configBuilder);
        }

        var config = configBuilder.Build();

        services.AddScoped<TIdetityDbContext>();
        services.AddSingleton<ILoginEndpointRouteConfiguration>(config);
        services.AddSingleton(config.JwtConfiguration);
        services.AddScoped<UserClaimsPrincipalFactory<TUser, IdentityRole>>();
        services.AddScoped<IIdentityJwtTokenGenerator<TUser>, JwtTokenGenerator<TUser>>();
        services.AddScoped<ILoginService, LoginService<TUser>>();

        services.AddIdentity<TUser, IdentityRole>()
            .AddEntityFrameworkStores<TIdetityDbContext>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<TUser, IdentityRole>>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static WebApplication UseVerisoftIdentity(this WebApplication app)
    {
        return UseVerisoftIdentity<IdentityUser, IdentityEntityFrameworkDbContext<IdentityUser>>(app);
    }

    public static WebApplication UseVerisoftIdentity<TUser, TIdetityDbContext>(this WebApplication app)
          where TUser : IdentityUser
          where TIdetityDbContext : IdentityDbContext<TUser>, IMigrable
    {
        var config = app.Services.GetService<ILoginEndpointRouteConfiguration>();

        using (var serviceScope = app.Services.CreateScope())
        {
            var migrable = serviceScope.ServiceProvider.GetService<TIdetityDbContext>();
            migrable.Migrate();
        }

        app.MapPost(config.DefaultEndpointRoute, async ([FromBody] LoginModel model, [FromServices] ILoginService accountService) =>
        {
            var token = await accountService.LoginAsync(model.Email, model.Password);
            if (token is not null)
            {
                return Results.Ok(new AuthResponse { Token = token });
            }

            return Results.Unauthorized();
        }).Produces(StatusCodes.Status200OK, typeof(AuthResponse))
          .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
          .Produces(StatusCodes.Status403Forbidden, typeof(ForbidResult));
        return app;
    }
}
