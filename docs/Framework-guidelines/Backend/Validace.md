#Validace
Na frontendu validovat můžeš, ale na backendu musíš.
Při validování se nelze spoléhat na to, že nám FE pošle správná data a zároveň všechny validace na frontendu nelze udělat. Proto je potřeba vstupy pro POST a PUT endpointy a navázané metody validovat.

Většinu validací provádíme v controlleru pomocí obecné ValidationService : IValidationService. Potřeba validace však může vyvstat i uvnitř business logiky v serivce, proto Metoda dané service vrací BusinessActionResult (blíže popsáno v [Návratové kódy a typy](/Framework-guidelines/Backend/Controllery/Návratové-kódy-a-typy)), čímž je zajištěno validní přeložení do IActionResult na controlleru.

BusinessActionResult využívá implicit operatory pro jednoduchou inicializaci objektu.

#Validace kontraktů

**Registrace**
`Verisoft.Core.Validation.ServiceCollectionExtensions.AddVerisoftFluentValidation<TValidator>(this IServiceCollection serviceCollection)`
- extension metoda nad IServiceCollection, kterou lze zavolat z InstallExtensions.cs nebo Program.cs
- type parametr je jeden z fluent validátorů (vysvětleno níže), ostatní jsou pak zaregistrovány z jeho Assembly
- tato metoda zaregistruje i ValidationService : IValidationService
Pro validaci používáme FluentValidation NuGet `using FluentValidation;`, přes jehož validátory drtivou většinu validací provádíme.

``` csharp
using FluentValidation;

namespace DemoApi.Application.Validators;

public class CustomerValidator: AbstractValidator<CustomerValidatedObject>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Name)
             .NotEmpty()
             .WithMessage(ErrorFactory.CustomerNameEmptyErrorMessage)
             .WithCode(ErrorFactory.CustomerNameEmptyErrorCode)
    }
}
```
-------------------

Pro každý objekt, který validuji a jeho podřízené objekty (v kardinalitě 1:1 i 1:n), je potřeba vytvořit samostatnou třídu validátoru, která dědí z AbstractValidatoru s type parametrem kontraktu : IValidatedObject, který vracíme z POST/PUT endpointu v 422.

**Prerekvizity pro validaci pomocí IValidationService**
- vytvoření třídy, která dědí od kontraktu, který přišel jako param endpointu a zároveň implementuje Verisoft.Core.Contracts.Validation.IValidatedObject interface
-- na této třídě je potřeba vydefinovat zanořené kolekce nebo objekty, které chci rovněž validovat a skrýt původní definici

``` csharp
public class ClientEditModel
{
    public string Name { get; set; }

    public string TradeId { get; set; }

    public HashSet<DocumentCreateModel> Documents { get; set; }

}
```

``` csharp
public class ClientValidatedObject : ClientEditModel, IValidatedObject
{
    public List<ValidationProblem> ValidationProblems { get; set; }

    public new HashSet<DocumentValidatedObject> Documents { get; set; }
}
```

- mapování všech "edit modelů" na "validated objecty" přes AutoMapper


``` csharp
cfg.CreateMap<ClientEditModel, ClientValidatedObject>();
cfg.CreateMap<DocumentCreateModel, DocumentValidatedObject>();
```

- inject IValidationService do controlleru

``` csharp
public class ClientController(IClientService clientService, IValidationService validationService)
```
---------------------
V controlleru pak vypadá validace takto


``` csharp
[HttpPut]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<ClientValidatedObject>))]
[AllowAnonymous]
public async Task<IActionResult> UpdateClientsAsync(ClientEditModel clientEditModel, int id)
{
    var validatedObject = await validationService.ValidateAsync<ClientValidatedObject, ClientEditModel>(clientEditModel, id);
    if (validatedObject.HasValidationErrors())
    {
        return validatedObject.ToActionResult();
    }

    var result = await clientService.UpdateClientAsync(clientEditModel, id);
    return result.ToActionResult();
}
```
**Příklad validátoru**

- validátor pro hlavní kontrakt má property `public int? Id { get; set; }` kdy int je PK entity kontraktu
-- slouží pro validaci existence entity, kdy lze využít `Verisoft.Core.Validation.Validators.ExistingEntityValidator`
 
``` csharp
RuleFor(x => Id)
     .SetAsyncValidator(new ExistingEntityValidator<ClientValidatedObject, ClientEntity, int>(clientRepository))
     .When(x => Id.HasValue)
     .DependentRules(() =>
     {
            // ... ostatní validace
     }
```
- lze využít i validátor pro kontrolu null inputu

 
``` csharp
RuleFor(x => x)
     .SetValidator(new NullInputValidator<ClientValidatedObject>())
     .DependentRules(() =>
     {
            // ... ostatní validace
     }
```

@<1EAAC8F1-E175-6326-A70E-86A045983945> - příklad celého validátoru vč těch výše popsaných

Pokud failne rule na nullinput controller vrátí 400
Pokud failne rule na existenci controller vrátí 404
Pokud failnou jiné rules, controller vrátí 422

#Přímé validace
V krajních případech můžeme dělat i validace přímo v business service, ale měli bychom se snažit vše odchytat přes [fluent validace v IValidationService](https://verisoftcz.visualstudio.com/Verisoft_framework/_wiki/wikis/Verisoft_framework.wiki/148/Validace?anchor=validace-kontrakt%C5%AF).

Často potřebujeme validovat, dostupnost resource. Například, pokud chceme dělat update entity, potřebujeme zajistit, že updatovaná entita v databázi existuje.

 
``` csharp
public async Task<BusinessActionResult<Customer>> UpdateCustomerAsync(int id, CustomerEditModel customerEditModel)
{
     var customerEntity = await customerRepository.GetAsync(id);

     if (customerEntity is null)
     {
         return ErrorFactory.NotFound(nameof(CustomerEntity), "Id", id.ToString());
     }

     // ... rest of update method code
}
```
Statické metody třídy Core.Common.BusinessResult.ErrorFactory umožňují vrátit BusinessActionResult naplněný objektem BusinessActionErrors a tím poskytnout potřebné informace z controlleru ven. Stejný kód, jako je výše se použije i pro vrácení 404 do GET endpointu.

Pro přímé validace lze využít metody NotFound(), BadRequest(), Forbidden() a protected metodu Error(), která je jediným způsobem, jak vytvořit objekt Core.Common.BusinessResult.BusinessActionErrors, který je v kolekci v objektu Errors.
Pro užití metody Error je potřeba vytvořit novou třídu, která od třídy ErrorFactory dědí. V této třídě by pak měly vzniknout nové metody, které Error vrací tak, aby bylo zajištěno, že veškeré validační hlášky řeší jedna třída.

_Příklad:_
``` csharp
public class ErrorFactory : Core.Common.BusinessResult.ErrorFactory
{
    private const string PayrollItemTypeInvoicingConfigurationDataCorruptionErrorCode = "PayrollItemTypeInvoicingConfigurationDataCorruption";
    private const string PayrollItemTypeInvoicingConfigurationDataCorruptionErrorMessage = "Payroll Item Type Invoicing Configuration for {payrollItemTypeCode} has been corrupted. Contact application support.";

    public static Error PayrollItemTypeInvoicingConfigurationDataCorruption(string payrollItemTypeCode)
    {
        return Error(PayrollItemTypeInvoicingConfigurationDataCorruptionErrorCode, PayrollItemTypeInvoicingConfigurationDataCorruptionErrorMessage, new { payrollItemTypeCode });
    }
}
```

Jak je popsáno v [Návratové kódy a typy](/Framework-guidelines/Backend/Controllery/Návratové-kódy-a-typy), tak veškeré Errory, které neobsahují kód, který je v jedné ze tří veřejných statických metod ErrorFactory se projeví návratovou hodnotou 422 na controlleru.