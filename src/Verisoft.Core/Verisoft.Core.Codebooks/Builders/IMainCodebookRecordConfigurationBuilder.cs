using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Configurations;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Builders;

public interface IMainCodebookRecordConfigurationBuilder
{
    public IMainCodebookRecordConfigurationBuilder ForType<T>(Action<CodebookRecordConfiguration> configure)
        where T : CodebookRecordType, new();

    IMainCodebookRecordConfigurationBuilder RegisterCodebooks(CodebookRegistry codebookRegistry);
}
