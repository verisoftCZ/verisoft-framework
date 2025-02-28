using Verisoft.CodebookApi.Core.Configurations;

namespace Verisoft.CodebookApi.Host.Builders;

public class CodebookRecordConfigurationBuilder(CodebookRecordConfiguration codebookRecordConfiguration)
    : ICodebookRecordConfigurationBuilder
{
    public ICodebookRecordConfigurationBuilder EndpointRoute(string endpointRoute)
    {
        endpointRoute = endpointRoute.Replace("Entity", string.Empty);
        codebookRecordConfiguration.CodebookRecordEndpointName = endpointRoute;
        return this;
    }

    public ICodebookRecordConfigurationBuilder TableName(string tableName)
    {
        tableName = tableName.Replace("Entity", string.Empty);
        codebookRecordConfiguration.CodebookRecordTableName = tableName;
        return this;
    }
}