namespace Verisoft.CodebookApi.Host.Builders;

public interface ICodebookRecordConfigurationBuilder
{
    ICodebookRecordConfigurationBuilder EndpointRoute(string endpointRoute);

    ICodebookRecordConfigurationBuilder TableName(string tableName);
}
