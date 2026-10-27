using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Common.LanguageServices;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.InstallExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodebooksForConsumer(this IServiceCollection serviceCollection, IEnumerable<Type> codebookTypes)
    {
        serviceCollection.RegisterCodebooks(codebookTypes);
        serviceCollection.AddLanguageServices();

        return serviceCollection;
    }

    public static void AddLanguageServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ILanguageProvider, HttpRequestLanguageProvider>();
    }

    public static CodebookRegistry RegisterCodebooks(this IServiceCollection services, IEnumerable<Type> codebookTypes)
    {
        var codebookRegistry = new CodebookRegistry("Records", "Codebook.");

        foreach (var type in codebookTypes)
        {
            if (!typeof(CodebookRecordType).IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"Type {type.Name} must inherit from {nameof(CodebookRecordType)}.");
            }

            typeof(CodebookRegistry)
                .GetMethod(nameof(CodebookRegistry.RegisterCodebook))!
                .MakeGenericMethod(type)
                .Invoke(codebookRegistry, null);
        }

        services.AddSingleton<ICodebookRegistry>(codebookRegistry);

        return codebookRegistry;
    }

    public static CodebookRegistry RegisterCodebooksFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        var types = CodebookDiscovery.DiscoverFromAssembly(assembly);
        return services.RegisterCodebooks(types);
    }
}