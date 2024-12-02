#Vytvoření nového komplexního číselníku

Přes CodebookController založit nový číselník do seznamu číselníků
- **Name:** Název číselníku př. Gender nebo Country
- **IsBasicType:** Pouze informativní pole, zda je číselník jednoduchý, nebo komplexní
- **HasHardcodedEnum:** Pouze informativní pole, zda k číselníku existuje Enum a při přidání záznamů do číselníku je tím pádem potřeba upravit i Enum v kódu

Do CodebookApi.Core.Entities.ComplexCodebookRecordsEntities přidat novou třídu, která dědí od CodebookRecordEntity a má doplňující properties
``` csharp
using CodebookApi.Core.Entities.BaseEntity;

namespace CodebookApi.Core.Entities.ComplexCodebookRecordsEntities;

public class CountryEntity : CodebookRecordEntity
{
    public string IsoAlpha2 { get; set; }

    public string IsoAlpha3 { get; set; }

    public string IsoNumeric3 { get; set; }
}
```
Do CodebookApi.Contracts.Models.<NázevČíselníku> přidat novou třídu, která má doplňující properties
``` csharp
namespace CodebookApi.Contracts.Models.Country;

public class Country
{
    public string IsoAlpha2 { get; set; }

    public string IsoAlpha3 { get; set; }

    public string IsoNumeric3 { get; set; }
}
```
Do CodebookApi.Contracts.Models.<NázevČíselníku> přidat novou třídu, která dědí od předchozí třídy a navíc je implementací interface ITenantContract
``` csharp
using CodebookApi.Contracts.Models.Base;
using Core.Contracts.Validations;

namespace CodebookApi.Contracts.Models.Country;

public class TenantCountry : Country, ITenantContract
{
    public bool IsForbidden { get; set; }

    public int TenantId { get; set; }

    public List<ValidationError> ValidationErrors { get; set; }

    public List<ValidationWarning> ValidationWarnings { get; set; }
}

```
Zaregistrovat novou entitu jako číselník do metody AddCodebooks() ve třídě CodebookApi.Host.InstallExtensions.CodebookRecordInstallExtension
``` csharp
namespace CodebookApi.Host.InstallExtensions;

public static class CodebookRecordInstallExtension
{
    public static IServiceCollection AddCodebooks(this IServiceCollection services)
    {
        return services.AddCodebookRecord<ICodebookApiDbContext>(c =>
        {
            c.AddComplexCodebook<CountryEntity, Country, TenantCountry>();
        });
    }
}
```
Spustit a aplikovat entity framework migraci