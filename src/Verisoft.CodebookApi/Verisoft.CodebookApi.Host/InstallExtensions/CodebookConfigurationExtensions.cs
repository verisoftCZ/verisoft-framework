using Verisoft.CodebookApi.Contracts.Models.Country;
using Verisoft.CodebookApi.Core.Entities.BasicCodebookRecordEntities;
using Verisoft.CodebookApi.Core.Entities.ComplexCodebookRecordsEntities;
using Verisoft.CodebookApi.Host.Builders;

namespace Verisoft.CodebookApi.Host.InstallExtensions;

public static class CodebookConfigurationExtensions
{
    public static void AddComplexCodebooks(this IMainCodebookRecordConfigurationBuilder config)
    {
        config.AddComplexCodebook<CountryEntity, Country, TenantCountry>();
    }

    public static void AddBasicCodebooks(this IMainCodebookRecordConfigurationBuilder config)
    {
        config.AddBasicCodebook<GenderEntity>();
    }
}
