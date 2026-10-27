using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Verisoft.Core.Codebooks.Authorization;
using Verisoft.Core.Codebooks.Builders;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.InstallExtensions;
using Verisoft.Core.Codebooks.Configurations;
using Verisoft.Core.Codebooks.Context;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Codebooks.Services;
using Verisoft.Core.Codebooks.Validators;
using Verisoft.Core.Common.Store;
using Verisoft.Core.Validation;

namespace Verisoft.Core.Codebooks.InstallExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodebooks(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        IEnumerable<Type> codebookTypes,
        Action<CodebookOptions>? configure = null)
    {
        var codebookRegistry = serviceCollection.RegisterCodebooks(codebookTypes);
        var codebookBuilder = new MainCodebookRecordConfigurationBuilder<ICodebookDbContext>(serviceCollection);

        codebookBuilder.RegisterCodebooks(codebookRegistry);

        codebookBuilder.Build();

        serviceCollection.RegisterCodebookRepositories();
        serviceCollection.RegisterCodebookServices();
        serviceCollection.AddVerisoftFluentValidation<CodebookValidator>();
        serviceCollection.AddLanguageServices();

        serviceCollection.AddOptions<CodebookAuthorizationConfiguration>()
            .Bind(configuration.GetSection(CodebookAuthorizationConfiguration.SectionName));

        var options = new CodebookOptions();
        configure?.Invoke(options);
        if (options.ConfigurePermissionChecker is null)
        {
            serviceCollection.AddScoped<ICodebookAuthorizationChecker, CodebookRoleChecker>();
        }
        else
        {
            options.ConfigurePermissionChecker(serviceCollection);
        }

        return serviceCollection;
    }

    private static void RegisterCodebookRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<ICodebookRepository, CodebookRepository>();
        serviceCollection.TryAddScoped<ILanguageRepository, LanguageRepository>();
        serviceCollection.TryAddScoped<IRepository<CodebookEntity, int>, CodebookRepository>();
        serviceCollection.TryAddScoped<IRepository<LanguageEntity, int>, LanguageRepository>();
    }

    private static void RegisterCodebookServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<ICodebookService, CodebookService>();
        serviceCollection.TryAddScoped<ILanguageService, LanguageService>();
        serviceCollection.TryAddScoped<IBulkStringValueService, BulkStringValueService>();
    }
}