
#Endpointy a routing
Standardní controller musí mít 5 základních endpointů
- **POST** - založení nového záznamu
apiUrl/<NázevControlleru>

``` csharp
[HttpPost]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<Customer>))]
public async Task<IActionResult> AddCustomerAsync(CustomerCreateModel customerCreateModel)
{
   var result = await customerService.AddCustomerAsync(customerCreateModel);
   return result.ToActionResult();
}
```

- **PUT** - update stávajícího záznamu
apiUrl/<NázevControlleru>/{id}
``` csharp
[HttpPut]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(BusinessActionErrors<Customer>))]
public async Task<IActionResult> UpdateCustomerAsync(CustomerEditModel customerEditModel, int id)
{
   var result = await customerService.UpdateCustomerAsync(customerEditModel, id);
   return result.ToActionResult();
}
```
- **DELETE** - smazání stávajícího záznamu
apiUrl/<NázevControlleru>/{id}
``` csharp
[HttpDelete]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
public async Task<IActionResult> DeleteCustomerAsync(int id)
{
   var result = await customerService.DeleteCustomerAsync(id);
   return result.ToActionResult();
}
```
- **GET** - získání jednoho záznamu na základě primárního klíče
apiUrl/<NázevControlleru>/{id}
-- v případě, že objekt s daným id nebyl nalezen vrací se 404

``` csharp
[HttpGet]
[Route("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BusinessActionErrors))]
public async Task<IActionResult> GetCustomerAsync(int id)
{
   var result = await customerService.GetCustomerAsync(id);
   return result.ToActionResult();
}
```
- **GET** - získání filtrovaných paged dat pro list záznamů (hlavně pro vykreslení do FE tabulky)
apiUrl/<NázevControlleru>?queryParams
   
``` csharp
[HttpGet]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerListData))]
public async Task<CustomertListData> GetCustomerListDataAsync([FromQuery] FilteredRequest<CustomerFilter> request)
{
   return await customerService.GetCustomerListDataAsync(request);
}
```
Routing dalších endpointů je hierarchický, tedy např.
`api/Customer/{id}/AssignToGroup/{groupId}`. Každý nový endpoint zdokumentujte
v OpenAPI a přidejte odpovídající integrační nebo unit test.

## Jak list endpoint propojuje filtrování a řazení

List endpoint nepředává filtrování ani řazení přímo do repository. Jednotlivé
vrstvy mají tyto odpovědnosti:

```text
HTTP GET /v2/Client?Offset=0&Limit=20&Sorts=Name,Asc&Filter.SearchField=acme
  → ClientController
      [FromQuery] FilteredRequest<ClientFilter>
  → BaseReadController.GetListAsync(request)
  → ClientService.GetClientListAsync(request)
  → ReadService.GetListAsync(request)
      ClientFilter → ClientSpecification → Expression<Func<ClientEntity, bool>>
      request.ParsedSorts → SortDefinition[]
  → ClientRepository.GetPagedSortAsync<ClientSort>(...)
      filter → WHERE
      ClientSort + requested sorts → ORDER BY / THEN BY
      Offset + Limit → Skip / Take
  → PagedData<ClientListItem> { Data, Total }
```

Konkrétní controller pouze přijme request a deleguje ho do base controlleru:

```csharp
[HttpGet]
public async Task<PagedData<ClientListItem>> GetClientListAsync(
    [FromQuery] FilteredRequest<ClientFilter> request)
    => await GetListAsync(request);
```

`BaseReadController.GetListAsync` zavolá `IReadService.GetListAsync`.
Konkrétní `ClientService` přitom používá `ReadService` s generickými typy
`ClientFilter`, `ClientSpecification`, `ClientSort` a `ClientEntity`. Application
vrstva proto nemusí ručně skládat SQL ani `IQueryable`; pouze předá request do
base služby.

`ReadService` namapuje `ClientFilter` na `ClientSpecification` pomocí
`ITypeMapper` a zavolá `SatisfiedBy()`, čímž vznikne LINQ expression pro
repository. Současně předá `request.ParsedSorts` a stránkovací hodnoty
`Offset`/`Limit`. Repository následně vytvoří EF Core dotaz a vrátí entity
spolu s celkovým počtem výsledků. `ReadService` entity namapuje na
`ClientListItem` a vrátí `PagedData<ClientListItem>`.

Podrobný popis formátu `FilteredRequest`, `Specification`, `BaseSort`,
defaultního řazení a pořadí operací v repository je v dokumentaci
[Filtrování, řazení a stránkování](../../03-Data/Databaze/Užití.md#filtrování-řazení-a-stránkování).

## Kompletní příklad – `Client`

Výše uvedený vzor v praxi implementuje
`Verisoft.DemoApi.Host.Controllers.V2.ClientController`. Celý řetězec
controller → base controller → service → repository vypadá takto:

```text
HTTP request
  → ClientController (Host)                – routing, autorizace, DTO ve/výstupu
    → CrudController<...> (Application)     – FluentValidation přes IValidationService,
                                               převod BusinessActionResult → IActionResult
      → IClientService / ClientService      – business logika (CrudService hooky)
        → IClientRepository / ClientRepository – EF Core dotazy/zápisy
          → DemoApiDbContext                – soft delete, audit pole, SaveChangesAsync
```

```csharp
[Authorize]
[ApiController]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
public class ClientController(
    IClientService clientService,
    IClientExportService clientExportService,
    IValidationService validationService)
    : CrudController<Client, ClientEditModel, ClientEditModel, ClientValidatedObject, ClientListItem, ClientFilter, ClientEntity, int>(
        clientService, clientExportService, validationService)
{
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult?> GetClientAsync(int id) => await GetAsync(id);

    [HttpGet]
    public async Task<PagedData<ClientListItem>> GetClientListAsync([FromQuery] FilteredRequest<ClientFilter> request)
        => await GetListAsync(request);

    [HttpPost]
    public async Task<IActionResult?> AddClientAsync(ClientEditModel clientEditModel) => await AddAsync(clientEditModel);

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult?> UpdateClientsAsync(ClientEditModel clientEditModel, int id) => await UpdateAsync(id, clientEditModel);

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult?> DeleteClientAsync(int id) => await DeleteAsync(id);

    [HttpPost]
    [Route("AddBulk")]
    public async Task<IActionResult> AddClientsBulkAsync(ClientEditModel[] clientEditModels) => await AddBulkAsync(clientEditModels);

    [HttpPost]
    [Route("DeleteBulk")]
    public async Task<IActionResult> DeleteClientsBulkAsync(List<int> ids) => await DeleteBulkAsync(ids);
}
```

Co se skutečně stane při `POST v2.0/Client` (`AddClientAsync`):

1. `ClientController` zdědil generický `[HttpPost] AddAsync` z
   `CrudController<...>` (viz výše) a jen ho volá pod konkrétním jménem
   metody kvůli routingu/OpenAPI popisu.
2. `CrudController.AddAsync` nejdřív zavolá
   `IValidationService.ValidateAsync<ClientValidatedObject, ClientEditModel>(editModel)`
   – tedy FluentValidation validaci vstupního modelu (viz
   [Validace](../Validace.md)); DTO `ClientEditModel` se pro tento účel
   mapuje na `ClientValidatedObject` (viz
   [Mapování a konfigurace AutoMapperu](../../01-Architektura/Mapovani-a-konfigurace-AutoMapperu.md)). Pokud validace
   selže, vrátí se rovnou `422`/`400` bez volání služby.
3. Pokud vstup projde, zavolá se `clientService.AddAsync(editModel)` –
   `ClientService` dědí z `CrudService<...>` (viz
   [Aplikační vrstva a base služby](../../01-Architektura/Aplikacni-vrstva-a-base-services.md)), takže proběhne sekvence
   `AddMapAsync` (mapování `ClientEditModel` → `ClientEntity` přes
   `ITypeMapper`, v `ClientService` přetížené jen o volání base
   implementace) → `GetAddValidationErrorsAsync` (business validace, v
   `ClientService` přetížená o vlastní kontrolu) → `repository.AddAsync` →
   `UnitOfWork.CommitAsync()` (uloží změny přes `DemoApiDbContext`, kde se
   automaticky doplní audit pole, viz [auditování](../../03-Data/Auditovani.md)).
4. Výsledný `BusinessActionResult<Client>` se v `CrudController`/
   `BaseReadController` převede na `IActionResult` přes `ToActionResult()`
   (viz [Návratové kódy a typy](Návratové-kódy-a-typy.md)).

Stejný vzor (base controller volá base service, ta hooky a repository) platí
pro `GetAsync`/`GetListAsync` (`ReadService`) i pro `UpdateAsync`/
`DeleteAsync`/bulk operace (`CrudService`).