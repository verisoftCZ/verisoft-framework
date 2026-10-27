using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Verisoft.Core.AspNet.Authentication;
using Verisoft.Core.AspNet.Authorization;
using Verisoft.Core.AspNet.Exceptions;
using Verisoft.Core.AspNet.Health;
using Verisoft.Core.Authentication;
using Verisoft.Core.Export;
using Verisoft.Core.Export.Strategies;

namespace Verisoft.Core.AspNet;

public static class InstallExtensions
{
    /// <summary>
    /// Registers default exception handlers.
    /// </summary>
    /// <remarks>
    /// Exception handlers execute in registration order.
    /// <see cref="GlobalExceptionHandler"/> must be registered last because it handles all uncaught exceptions.
    /// </remarks>
    public static void AddVerisoftExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<InvalidSortPropertyExceptionHandler>();
        services.AddExceptionHandler<CrawlerExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void AddApiHeaderAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthConfig, DefaultAuthConfig>();
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
        if (string.IsNullOrWhiteSpace(config.Secret))
        {
            throw new InvalidOperationException("Authentication secret is missing. Configure 'apiSecret' with a strong key.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(config.Secret);
        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException("Authentication secret must be longer than 32 bytes for HS256.");
        }

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
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
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
        if (string.IsNullOrEmpty(config.ConnectionString))
        {
            throw new ArgumentException("Connection string must be provided.", nameof(config.ConnectionString));
        }

        var builder = services.AddHealthChecks();

        return config.Provider switch
        {
            HealthDatabaseProvider.SqlServer =>
                builder.AddSqlServer(
                    connectionString: config.ConnectionString,
                    name: config.DatabaseName,
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["db", "sql", "sqlserver"]),

            HealthDatabaseProvider.PostgreSql =>
                builder.AddNpgSql(
                    connectionString: config.ConnectionString,
                    healthQuery: "SELECT 1",
                    name: config.DatabaseName,
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["db", "pgsql", "postgres"]),

            _ => throw new NotSupportedException($"Health provider '{config.Provider}' is not supported."),
        };
    }
}