using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Verisoft.CodebookApi.Host.Context;
using Verisoft.CodebookApi.Host.Helpers;
using Verisoft.Core.ApplicationInsights;
using Verisoft.Core.AspNet;
using Verisoft.Core.Codebooks.Common.Extensions;
using Verisoft.Core.Codebooks.Context;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Data.EntityFramework.Configuration;
using Verisoft.Core.OpenApi.Filters;
using Verisoft.Core.OpenApi.OpenApi;
using Verisoft.Core.TypeMapper;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;

namespace Verisoft.CodebookApi.Host.InstallExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddDemoCodebookApi(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddProblemDetails();
        RegisterDatabase(serviceCollection);
        RegisterServices(serviceCollection);
        RegisterRepositories(serviceCollection);
        RegisterAuthentication(serviceCollection, configuration);
        RegisterMapper(serviceCollection);
        RegisterMonitoring(serviceCollection, configuration);
        RegisterHealthChecks(serviceCollection, configuration);
        AddOpenApiSpecification(serviceCollection);
        serviceCollection.AddVerisoftExceptionHandlers();
    }

    private static void AddOpenApiSpecification(IServiceCollection serviceCollection)
    {
        serviceCollection.AddVerisoftOpenApiSpecification(
            swaggerOptions: options =>
            {
                options.SchemaFilter<NullableObjectSchemaFilter>("Demo");
                options.MapCodebookRecordsFromAssemblies(typeof(Accessibility).Assembly);
            },
            apiInfo: info =>
            {
                info.Title = "Demo.CodebookApi";
                info.Description = "Web API for managing Codebooks.";
            });
    }

    private static void RegisterAuthentication(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build());

        serviceCollection.AddVerisoftAuth(c =>
        {
            c.Secret = configuration.GetValue<string>("apiSecret");
            c.ValidateLifetime = false;
        });
    }

    private static void RegisterMonitoring(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddVerisoftApplicationInsights(configuration, new AIOptions()
        {
            EnableSqlCommandTextTracking = true,
            EnableHangfireTracking = false,
        });
    }

    private static void RegisterHealthChecks(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddVerisoftHealth(c =>
        {
            c.ConnectionString = configuration["Database:ConnectionString"];
            c.DatabaseName = "hackaton";
        });
    }

    private static void RegisterRepositories(IServiceCollection serviceCollection)
    {
    }

    private static void RegisterMapper(IServiceCollection serviceCollection)
    {
        var mapper = AutoMapperFactory.CreateMapper();
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddScoped<ITypeMapper, TypeMapper>();
    }

    private static void RegisterDatabase(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IDatabaseConfig, DatabaseConfig>();
        serviceCollection.TryAddScoped<DemoApiCodebookDbContext>();
        serviceCollection.TryAddScoped<IDemoApiCodebookDbContext>(sp => sp.GetRequiredService<DemoApiCodebookDbContext>());
        serviceCollection.TryAddScoped<ICodebookDbContext>(sp => sp.GetRequiredService<DemoApiCodebookDbContext>());
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
    }
}