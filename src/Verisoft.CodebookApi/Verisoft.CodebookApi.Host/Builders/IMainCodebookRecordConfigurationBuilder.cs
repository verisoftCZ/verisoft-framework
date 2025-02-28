using Verisoft.CodebookApi.Core.Entities.BaseEntity;

namespace Verisoft.CodebookApi.Host.Builders;

public interface IMainCodebookRecordConfigurationBuilder
{
    string CodebookRecordTablePrefix { set; }

    string CodebookRecordRoutePrefix { set; }

    ICodebookRecordConfigurationBuilder AddComplexCodebook<TCodebookRecord, TAdditionalProperties, TTenantValue>()
        where TCodebookRecord : class, ICodebookRecordEntity, new()
        where TAdditionalProperties : class
        where TTenantValue : class;

    ICodebookRecordConfigurationBuilder AddBasicCodebook<TCodebookRecord>()
        where TCodebookRecord : class, ICodebookRecordEntity, new();
}
