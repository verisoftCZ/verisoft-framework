using System;
using System.Text;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Verisoft.Core.AspNet.ApplicationInsights;
using Verisoft.Core.AspNet.Authentication;
using Verisoft.Core.AspNet.Authorization;
using Verisoft.Core.AspNet.Hangfire;
using Verisoft.Core.AspNet.Health;
using Verisoft.Core.Authentication;
using Verisoft.Core.Common.Logging;
using Verisoft.Core.ExportStrategies;
using Verisoft.Core.ExportStrategies.Strategies;

namespace Verisoft.Core.AspNet;

public static class InstallExtensions
{
    public static void AddApiHeaderAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthConfig, DefaultAuthConfig>();
    }

    public static void AddVerisoftApplicationInsights(this IServiceCollection services, IConfiguration configuration, AIOptions options)
    {
        services.AddApplicationInsightsTelemetry(configuration);
        if (options != null && options.EnableSqlCommandTextTracking)
        {
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });
        }

        if (options != null && !options.EnableHangfireTracking)
        {
            services.AddApplicationInsightsTelemetryProcessor<NoSqlTelemetryProcessor>();
        }

        services.AddSingleton<AIConfig>();
        services.AddTransient<IBusinessTracker, AITracker>();
    }

    public static void AddVerisoftAuth(this IServiceCollection services, Action<IdentityAuthenticationOptions> options)
    {
        var config = new IdentityAuthenticationOptions();
        config.ValidateAudience = false;
        options?.Invoke(config);
        services.AddVerisoftAuth(config);
    }

    public static void AddVerisoftAuth(this IServiceCollection services, IAuthConfiguration config)
    {
        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = config.ValidateAudience,
                    ValidateAudience = config.ValidateAudience,
                    RequireAudience = config.ValidateAudience,
                    ValidateIssuerSigningKey = config.ValidateIssuerSigningKey,
                    ValidateActor = config.ValidateActor,
                    ValidateLifetime = config.ValidateLifetime,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddHttpContextAccessor();
        services.AddSingleton(config);
        services.AddScoped<IUserContext, HttpUserContext>();
    }

    public static void AddDefaultExportStrategies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IExportStrategy<>), typeof(ExcelExportStrategy<>));
        serviceCollection.AddScoped(typeof(IExportStrategy<>), typeof(JsonExportStrategy<>));
        serviceCollection.AddScoped(typeof(IExportStrategy<>), typeof(XmlExportStrategy<>));
        serviceCollection.AddScoped(typeof(IExportStrategy<>), typeof(CsvExportStrategy<>));

        serviceCollection.TryAddScoped(typeof(ExportStrategyResolver<>));
    }

    public static IHealthChecksBuilder AddVerisoftHealth(this IServiceCollection services, Action<HealthConfiguration> options)
    {
        var config = new HealthConfiguration();
        options?.Invoke(config);
        return services.AddHealthChecks()
            .AddSqlServer(
                connectionString: config.ConnectionString,
                name: config.DatabaseName,
                failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                tags: ["db", "sql", "sqlserver"]);
    }
}