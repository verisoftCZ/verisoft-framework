using Microsoft.Extensions.DependencyInjection;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Configurations;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Codebooks.Services;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.Codebooks.Builders;

public class MainCodebookRecordConfigurationBuilder<TContext>(IServiceCollection services)
    : IMainCodebookRecordConfigurationBuilder
     where TContext : IUnitOfWork
{
    private readonly MainCodebookRecordConfiguration codebookRecordConfiguration = new();

    private readonly Dictionary<Type, Action<CodebookRecordConfiguration>> customConfigurations = [];

    private CodebookRegistry? codebookRegistry;

    public IMainCodebookRecordConfigurationBuilder ForType<TCodebookRecordType>(Action<CodebookRecordConfiguration> configure)
    where TCodebookRecordType : CodebookRecordType, new()
    {
        customConfigurations[typeof(TCodebookRecordType)] = configure;
        return this;
    }

    public IMainCodebookRecordConfigurationBuilder RegisterCodebooks(CodebookRegistry codebookRegistry)
    {
        this.codebookRegistry = codebookRegistry;
        return this;
    }

    public MainCodebookRecordConfiguration Build()
    {
        ArgumentNullException.ThrowIfNull(codebookRegistry, nameof(codebookRegistry));

        codebookRecordConfiguration.CodebookRecordTablePrefix = codebookRegistry.CodebookRecordTablePrefix;
        codebookRecordConfiguration.CodebookRecordRoutePrefix = codebookRegistry.CodebookRecordRoutePrefix;

        foreach (var codebookType in codebookRegistry.GetRegisteredCodebooks())
        {
            customConfigurations.TryGetValue(codebookType, out var customConfig);

            AddCodebook(codebookType, customConfig);
        }

        services.AddSingleton(codebookRecordConfiguration);

        return codebookRecordConfiguration;
    }

    private void RegisterCodebookServices<TCodebookRecordType>(
        Type? customRepositoryType = null,
        Type? customRepositoryInterfaceType = null,
        Type? customTranslationRepositoryType = null,
        Type? customTranslationRepositoryInterfaceType = null,
        Type? customCodebookRecordServiceType = null,
        Type? customCodebookRecordServiceInterfaceType = null,
        Type? customStringValueServiceType = null,
        Type? customStringValueServiceInterfaceType = null)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        RegisterService<ICodebookRecordRepository<TCodebookRecordType>, CodebookRecordRepository<TContext, TCodebookRecordType>>(customRepositoryType, customRepositoryInterfaceType);
        RegisterService<ITranslationRepository<TCodebookRecordType>, TranslationRepository<TContext, TCodebookRecordType>>(customTranslationRepositoryType, customTranslationRepositoryInterfaceType);
        RegisterService<ICodebookRecordService<TCodebookRecordType>, CodebookRecordService<TCodebookRecordType>>(customCodebookRecordServiceType, customCodebookRecordServiceInterfaceType);
        RegisterService<IStringValueService<TCodebookRecordType>, StringValueService<TCodebookRecordType>>(customStringValueServiceType, customStringValueServiceInterfaceType);
    }

    private void RegisterService<TInterface, TDefaultImplementation>(
        Type? customType = null,
        Type? customInterfaceType = null)
        where TInterface : class
        where TDefaultImplementation : class, TInterface
    {
        if (customType == null)
        {
            services.AddScoped<TInterface, TDefaultImplementation>();
        }
        else
        {
            services.AddScoped(typeof(TInterface), customType);

            if (customInterfaceType != null)
            {
                services.AddScoped(customInterfaceType, customType);
            }
        }
    }

    private void AddCodebook(Type codebookType, Action<CodebookRecordConfiguration>? customConfig)
    {
        var options = new CodebookRecordConfiguration
        {
            CodebookRecordType = codebookType,
        };

        customConfig?.Invoke(options);

        codebookRecordConfiguration.CodebookRecordConfiguration.Add(options);
        var method = GetType().GetMethod(nameof(RegisterCodebookServices), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .MakeGenericMethod(codebookType);

        method?.Invoke(
            this,
            [
            options.CustomRepositoryType,
            options.CustomRepositoryInterfaceType,
            options.CustomTranslationRepositoryType,
            options.CustomTranslationRepositoryInterfaceType,
            options.CustomCodebookRecordServiceType,
            options.CustomCodebookRecordServiceInterfaceType,
            options.CustomStringValueServiceType,
            options.CustomStringValueServiceInterfaceType,
            ]);
    }
}
