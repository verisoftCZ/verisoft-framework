using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Verisoft.Core.AspNet;
using Verisoft.Core.AspNet.Authentication;
using Verisoft.Core.AspNet.Filters;
using Verisoft.Core.AspNet.OpenApi;
using Verisoft.Core.Authentication;
using Verisoft.Core.Authentication.Permissions;
using Verisoft.Core.BlobStorage.InstallExtensions;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Data.EntityFramework.Configuration;
using Verisoft.Core.MassTransit;
using Verisoft.Core.Pdf.Extensions;
using Verisoft.Core.Template.Extensions;
using Verisoft.Core.TypeMapper;
using Verisoft.Core.Validation;
using Verisoft.DemoApi.Application.Helpers;
using Verisoft.DemoApi.Application.Services;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Application.Validators.Client;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;
using Verisoft.DemoApi.Data.EF.Repositories;
using Verisoft.DemoApi.Host.Consumers;
using Verisoft.DemoApi.Host.HangfireJobs;

namespace Verisoft.DemoApi.Host.InstallExtensions;

public static class InstallExtensions
{
    private const string MitLicenceUrl = "https://opensource.org/licenses/MIT";

    public static void AddOpenApiSpecification(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddVerisoftOpenApiSpecification(
            apiVersioningOptions: options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);
            },
            apiExplorerOptions: options =>
            {
                options.GroupNameFormat = "'v'V";
            },
            swaggerOptions: options =>
            {
                options.SchemaFilter<NullableObjectSchemaFilter>("Verisoft");
            },
            apiInfo: info =>
            {
                info.Title = "Verisoft Demo.API";
                info.Description = "Verisoft demo API for showcase purposes";
                info.Contact = new OpenApiContact { Name = "Jack Szabo", Email = "jack.szabo@verisoft.cz" };
                info.License = new OpenApiLicense { Name = "MIT", Url = new Uri(MitLicenceUrl) };
            });
    }

    public static void AddDemoApi(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        RegisterServices(serviceCollection);
        RegisterDatabase(serviceCollection);
        RegisterRepositories(serviceCollection);
        RegisterAuthentication(serviceCollection, configuration);
        RegisterMapper(serviceCollection);
        RegisterMonitoring(serviceCollection, configuration);
        RegisterHealthChecks(serviceCollection, configuration);
        RegisterHangfire(serviceCollection, configuration);
        RegisterMassTransit(serviceCollection, configuration);
        RegisterBlobStorage(serviceCollection, configuration);
        RegisterPdfGenerator(serviceCollection);
        RegisterTemplates(serviceCollection);
        serviceCollection.AddVerisoftFluentValidation<ClientValidator>();
    }

    private static void RegisterTemplates(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTemplateService();
    }

    private static void RegisterPdfGenerator(IServiceCollection serviceCollection)
    {
        serviceCollection.AddPdfService();
    }

    private static void RegisterBlobStorage(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddBlobStorage(configuration);
    }

    private static void RegisterHangfire(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddHangfire(options =>
        {
            options.HangfireSchemaName = "Hangfire";
            options.ConnectionString = configuration["Database:ConnectionString"];
        });
        HangfireJobsConfig hangfireJobsConfig = new HangfireJobsConfig(configuration);
        serviceCollection.AddSingleton(hangfireJobsConfig);
    }

    private static void RegisterHealthChecks(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddVerisoftHealth(c =>
        {
            c.ConnectionString = configuration["Database:ConnectionString"];
            c.DatabaseName = "hackaton";
        });
    }

    private static void RegisterMonitoring(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddVerisoftApplicationInsights(configuration, new Verisoft.Core.AspNet.ApplicationInsights.AIOptions()
        {
            EnableSqlCommandTextTracking = true,
            EnableHangfireTracking = false,
        });
    }

    private static void RegisterMapper(IServiceCollection serviceCollection)
    {
        IMapper mapper = AutoMapperFactory.CreateMapper();
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddScoped<ITypeMapper, TypeMapper>();
    }

    private static void RegisterAuthentication(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthorization(c =>
        {
            c.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build();
        });

        serviceCollection.AddVerisoftAuth(c => { c.Secret = configuration.GetValue<string>("apiSecret"); });
        serviceCollection.AddScoped<ITokenService, TokenService>();
        serviceCollection.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        serviceCollection.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void RegisterRepositories(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IClientRepository, ClientRepository>();
        serviceCollection.TryAddScoped<IUserRepository, UserRepository>();
        serviceCollection.TryAddScoped<IDocumentRepository, DocumentRepository>();
        serviceCollection.TryAddScoped<IBlobRepository, BlobRepository>();
    }

    private static void RegisterDatabase(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IDatabaseConfig, DatabaseConfig>();
        serviceCollection.TryAddScoped<IDemoApiDbContext, DemoApiDbContext>();
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IClientService, ClientService>();
        serviceCollection.TryAddScoped<IUserService, UserService>();
        serviceCollection.TryAddScoped<IDocumentService, DocumentService>();
        serviceCollection.TryAddScoped<IIdentityService, IdentityService>();
        serviceCollection.TryAddScoped<IDemoPdfService, DemoPdfService>();
        serviceCollection.TryAddScoped<IPermissionService, PermissionService>();
        serviceCollection.TryAddScoped<IUserContext, HttpUserContext>();
        serviceCollection.AddDefaultExportStrategies();
    }

    private static void RegisterMassTransit(IServiceCollection services, IConfiguration configuration)
    {
        services.AddVerisoftServiceBus<Program>(
            configuration,
            options
                => options.ConfigureConsumer<CreateDocumentConsumer>()
                    .MaxRetries(3)
                    .ConcurrencyLimit(1));
    }
}