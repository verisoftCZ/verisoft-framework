# Aplikační vrstva (Application)

`Verisoft.DemoApi.Application` obsahuje business logiku Demo API. Vrstva stojí
mezi `Host` (controllery) a `Data.EF`/`Common` (repozitáře, entity) a je
jediné místo, kam patří skutečná business logika – controllery i repozitáře
mají zůstat tenké (viz [Controllery](../02-API/Controllery.md) a
[Repository pattern](../03-Data/Databaze/Repository-pattern.md)).

```text
Verisoft.DemoApi.Application/
├── Controllers/    base controllery (BaseReadController, CrudController)
├── Services/       business služby + Services/Interfaces
├── Validators/     FluentValidation validátory (viz ../02-API/Validace.md)
└── Helpers/        pomocné třídy, např. AutoMapperFactory
```

## Base služby (`Verisoft.Core.AspNet.Services`)

Framework poskytuje dvě generické base třídy pro služby nad entitou, na
kterých stojí prakticky každá CRUD služba v Demo API:

- **`ReadService<TContract, TListItem, TFilter, TSpecification, TSort, TEntity, TKey>`**
  – jen čtení: `ExistsAsync`, `GetAsync`, `GetProjectedAsync`, `GetListAsync`,
  `GetProjectedListAsync`. Mapování entita → DTO řeší přes `ITypeMapper`
  (`MapToContract`/`MapToListItems`), filtr → specifikace mapuje také přes
  `ITypeMapper` (`TSpecification`, viz [Užití](../03-Data/Databaze/Užití.md)).
- **`CrudService<TContract, TCreateModel, TEditModel, TListItem, TFilter, TSpecification, TSort, TEntity, TKey>`**
  (dědí z `ReadService`) – přidává zápis: `AddAsync`, `UpdateAsync`,
  `RemoveAsync`, `AddBulkAsync`, `RemoveBulkAsync`.

`CrudService` má u každé operace sadu **virtuálních hooků**, které lze
přetížit bez nutnosti přepisovat celou metodu (např. vlastní validace nebo
mapování jen pro jednu entitu):

| Operace | Hooky (v pořadí volání) |
|---|---|
| `AddAsync` | `AddPreMapAsync` → `AddMapAsync` → `GetAddValidationErrorsAsync` → `PreAddAsync` → `repository.AddAsync` → `PostAddAsync` → `Commit` → `PostAddCommitAsync` |
| `UpdateAsync` | `GetUpdateValidationErrorsAsync` → `PreUpdateAsync` → `UpdateMapAsync` → `PostUpdateAsync` → `Commit` → `PostUpdateCommitAsync` |
| `RemoveAsync` | `GetRemoveValidationErrorsAsync` → `PreRemoveAsync` → `repository.Remove` → `PostRemoveAsync` → `Commit` → `PostRemoveCommitAsync` |

Výchozí implementace `AddMapAsync`/`UpdateMapAsync` používá `ITypeMapper.Map`
(tedy AutoMapper, viz [Mapování a konfigurace AutoMapperu](Mapovani-a-konfigurace-AutoMapperu.md)) –
stačí mít nakonfigurovaný mapping profil a hooky vůbec nepřepisovat.
`GetAddValidationErrorsAsync`/`GetUpdateValidationErrorsAsync`/
`GetRemoveValidationErrorsAsync` slouží pro business validaci (na rozdíl od
FluentValidation validace vstupního modelu, která běží v controlleru před
zavoláním služby, viz [Validace](../02-API/Validace.md)) – např. kontrola
byznysových invariantů, které vyžadují dotaz do databáze.

## Konkrétní služba (příklad `ClientService`)

```csharp
public class ClientService(
    IClientRepository clientRepository,
    ITypeMapper typeMapper)
    : CrudService<Client, ClientEditModel, ClientEditModel, ClientListItem, ClientFilter, ClientSpecification, ClientSort, ClientEntity, int>(typeMapper, clientRepository),
      IClientService
{
    public async Task<BusinessActionResult<Client>> AddClientAsync(ClientEditModel clientEditModel) => await AddAsync(clientEditModel);

    public async Task<BusinessActionResult<Client>> GetClientAsync(int id) => await GetAsync(id);

    public async Task<PagedData<ClientListItem>> GetClientListAsync(FilteredRequest<ClientFilter> request) => await GetListAsync(request);

    public async Task<BusinessActionResult> RemoveClientAsync(int id) => await RemoveAsync(id);

    public async Task<BusinessActionResult<Client>> UpdateClientAsync(ClientEditModel clientEditModel, int id) => await UpdateAsync(id, clientEditModel);

    // Vlastní hook: doplňkové zpracování při mapování na entitu (asynchronní práce navíc, pokud je potřeba).
    protected override async Task<ClientEntity> AddMapAsync(ClientEditModel? createModel)
    {
        var mapped = await base.AddMapAsync(createModel);
        return mapped;
    }

    // Vlastní hook: business validace navíc k FluentValidation validaci vstupu.
    protected override async Task<BusinessActionErrors> GetAddValidationErrorsAsync(ClientEditModel createModel)
    {
        if (string.IsNullOrEmpty(createModel.Name) || createModel.Name.Length < 10)
        {
            return ErrorFactory.UnprocessableEntity("Clients must have really long names of at least 10 characters", new { createModel.Name });
        }

        return await base.GetAddValidationErrorsAsync(createModel);
    }
}
```

- Generické parametry třídy odpovídají DTO/entitě v pořadí `TContract,
  TCreateModel, TEditModel, TListItem, TFilter, TSpecification, TSort,
  TEntity, TKey`. Pokud create a edit model sdílí stejné DTO (jak je tomu u
  `Client`), použije se stejný typ na obou pozicích.
- Vlastní veřejné metody služby (`AddClientAsync`, `GetClientHistoryListAsync`
  apod.) tvoří konkrétní `IClientService` – to je interface, který
  controller skutečně injektuje (ne generický `ICrudService<...>` přímo).
- Konkrétní repozitář (`IClientRepository`) se injektuje přes konstruktor a
  je současně předán do base třídy jako `IRepository<TEntity, TKey>`.

Registrace probíhá standardně v `InstallExtensions` (viz
[Struktura backendu](Backend.md)):

```csharp
serviceCollection.TryAddScoped<IClientService, ClientService>();
```

Propojení base controller → base service → repository → mapování je
podrobně na jednom konkrétním příkladu popsáno v
[Controllery: Endpointy a routing](../02-API/Controllery/Endpointy-a-routing.md).
