using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Application.Services;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Application.Validators;
using Verisoft.CodebookApi.Core.Configurations;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Database.Configurations;
using Verisoft.CodebookApi.Database.Context;
using Verisoft.CodebookApi.Database.Repositories;
using Verisoft.CodebookApi.Host.Builders;
using Verisoft.Core.AspNet;
using Verisoft.Core.AspNet.Filters;
using Verisoft.Core.AspNet.OpenApi;
using Verisoft.Core.Authentication.Permissions;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Data.EntityFramework.Configuration;
using Verisoft.Core.TypeMapper;
using Verisoft.Core.Validation;

namespace Verisoft.CodebookApi.Host.InstallExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddCodebookApi(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        RegisterServices(serviceCollection);
        RegisterDatabase(serviceCollection);
        RegisterRepositories(serviceCollection);
        RegisterAuthentication(serviceCollection, configuration);
        RegisterMapper(serviceCollection);
        RegisterMonitoring(serviceCollection, configuration);
        RegisterHealthChecks(serviceCollection, configuration);
        AddCodebooks(serviceCollection);
        AddOpenApiSpecification(serviceCollection);
        serviceCollection.AddVerisoftFluentValidation<TranslationValidator>();
    }

    private static void AddOpenApiSpecification(IServiceCollection serviceCollection)
    {
        serviceCollection.AddVerisoftOpenApiSpecification(
    swaggerOptions: options =>
    {
        options.SchemaFilter<NullableObjectSchemaFilter>("Verisoft");
    },
    apiInfo: info =>
    {
        info.Title = "Verisoft.CodebookApi";
        info.Description = "Web API for managing Codebooks.";
    });
    }

    private static IServiceCollection AddCodebooks(IServiceCollection serviceCollection)
    {
        return serviceCollection.AddCodebookRecord<ICodebookApiDbContext>(config =>
        {
            config.CodebookRecordRoutePrefix = "Records";
            config.CodebookRecordTablePrefix = "Codebook.";
            config.AddBasicCodebooks();
            config.AddComplexCodebooks();
        });
    }

    private static IServiceCollection AddCodebookRecord<TContext>(this IServiceCollection serviceCollection, Action<IMainCodebookRecordConfigurationBuilder> config)
       where TContext : IUnitOfWork
    {
        var builder = new MainCodebookRecordConfigurationBuilder<TContext>(serviceCollection);
        config(builder);
        var configuration = builder.Build();
        serviceCollection.AddSingleton<IMainCodebookRecordConfiguration>(configuration);
        serviceCollection.AddScoped<ICodebookRecordSchemaManager, CodebookRecordSchemaManager>();

        return serviceCollection;
    }

    private static void RegisterAuthentication(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthorization(c =>
        {
            c.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build();
        });

        serviceCollection.AddVerisoftAuth(c =>
        {
            c.Secret = configuration.GetValue<string>("apiSecret");
        });
        serviceCollection.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        serviceCollection.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void RegisterMonitoring(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddVerisoftApplicationInsights(configuration, new Verisoft.Core.AspNet.ApplicationInsights.AIOptions()
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
        serviceCollection.TryAddScoped<ICodebookRepository, CodebookRepository>();
        serviceCollection.TryAddScoped<ILanguageRepository, LanguageRepository>();
        serviceCollection.TryAddScoped<ITranslationRepository, TranslationRepository>();
        serviceCollection.TryAddScoped<ITenantValueRepository, TenantValueRepository>();
        serviceCollection.TryAddScoped<IRepository<CodebookEntity, int>, CodebookRepository>();
        serviceCollection.TryAddScoped<IRepository<LanguageEntity, int>, LanguageRepository>();
        serviceCollection.TryAddScoped<ITenantRepository, SingleTenantRepository>();

        // Switch SingleTenantRepository with MultiTenantRepository and implement it's logic to enable multi-tenancy
        // serviceCollection.TryAddScoped<ITenantRepository, MultiTenantRepository>();
    }

    private static void RegisterMapper(IServiceCollection serviceCollection)
    {
        IMapper mapper = AutoMapperFactory.CreateMapper();
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddScoped<ITypeMapper, TypeMapper>();
    }

    private static void RegisterDatabase(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IDatabaseConfig, DatabaseConfig>();
        serviceCollection.TryAddScoped<ICodebookApiDbContext, CodebookApiDbContext>();
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<ICodebookService, CodebookService>();
        serviceCollection.TryAddScoped<ILanguageService, LanguageService>();
        serviceCollection.TryAddScoped<ITranslationService, TranslationService>();
    }
}
