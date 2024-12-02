#Vytvoření nového jednoduchého číselníku

Přes CodebookController založit nový číselník do seznamu číselníků
- **Name:** Název číselníku př. Gender nebo Country
- **IsBasicType:** Pouze informativní pole, zda je číselník jednoduchý, nebo komplexní
- **HasHardcodedEnum:** Pouze informativní pole, zda k číselníku existuje Enum a při přidání záznamů do číselníku je tím pádem potřeba upravit i Enum v kódu

Do CodebookApi.Core.Entities.BasicCodebookRecordEntities přidat novou třídu, která dědí od CodebookRecordEntity
``` csharp
using CodebookApi.Core.Entities.BaseEntity;

namespace CodebookApi.Core.Entities.BasicCodebookRecordEntities;

public class GenderEntity : CodebookRecordEntity
{
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
            c.AddBasicCodebook<GenderEntity>();
        });
    }
}
```
Spustit a aplikovat entity framework migraci