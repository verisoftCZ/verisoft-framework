using Verisoft.CodebookApi.Application.Services;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Core.Configurations;
using Verisoft.CodebookApi.Core.Entities.BaseEntity;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Database.Configurations;
using Verisoft.CodebookApi.Database.Repositories;
using Verisoft.Core.Common.Store;

namespace Verisoft.CodebookApi.Host.Builders;

public class MainCodebookRecordConfigurationBuilder<TContext>(IServiceCollection services)
    : IMainCodebookRecordConfigurationBuilder
     where TContext : IUnitOfWork
{
    private readonly MainCodebookRecordConfiguration codebookRecordConfiguration = new();

    public string CodebookRecordTablePrefix { set => codebookRecordConfiguration.CodebookRecordTablePrefix = value; }

    public string CodebookRecordRoutePrefix { set => codebookRecordConfiguration.CodebookRecordRoutePrefix = value; }

    public ICodebookRecordConfigurationBuilder AddBasicCodebook<TCodebookRecord>()
        where TCodebookRecord : class, ICodebookRecordEntity, new()
    {
        return AddComplexCodebook<TCodebookRecord, BasicDefaultContract, BasicTenantContract>();
    }

    public ICodebookRecordConfigurationBuilder AddComplexCodebook<TCodebookRecord, TAdditionalProperties, TTenantValue>()
        where TCodebookRecord : class, ICodebookRecordEntity, new()
        where TAdditionalProperties : class
        where TTenantValue : class
    {
        var codebookRecordConfig = new CodebookRecordConfiguration { CodebookRecordType = typeof(TCodebookRecord), PropContract = typeof(TAdditionalProperties), TenantContract = typeof(TTenantValue) };
        services.AddScoped<ICodebookRecordRepository<TCodebookRecord>, CodebookRecordRepository<TContext, TCodebookRecord>>();
        services.AddScoped<ICodebookRecordService<TCodebookRecord>, CodebookRecordService<TCodebookRecord>>();
        services.AddScoped<IStringValueService<TCodebookRecord>, StringValueService<TCodebookRecord>>();
        codebookRecordConfiguration.CodebookRecordConfiguration.Add(codebookRecordConfig);
        return new CodebookRecordConfigurationBuilder(codebookRecordConfig);
    }

    public MainCodebookRecordConfiguration Build()
    {
        return codebookRecordConfiguration;
    }
}
