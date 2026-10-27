#Užití
**do databáze se přistupuje výhradně přes repozitáře**

Repozitář (`Verisoft.Core.Common.Store.IRepository<TEntity, TKey>`) poskytuje
v základu většinu potřebných metod, které lze i overridnout:

- `Task<bool>` **ExistsAsync**(TKey id) - ověření existence entity dle jejího primárního klíče

- `Task<bool>` **AnyAsync**(Expression<Func<TEntity, bool>> filter) - ověření existence záznamu odpovídajícího filtru

- `Task<TEntity?>` **GetAsync**(TKey id) - získání entity dle jejího primárního klíče

- `Task<TProjection?>` **GetProjectedAsync**<TProjection>(TKey id, Expression<Func<TEntity, TProjection>> selector) - získání promítnuté (projected) entity dle primárního klíče

- `Task<(IEnumerable<TEntity> Items, int Count)>` **GetPagedSortAsync**<TSort>(int offset, int limit, Expression<Func<TEntity, bool>>? filter, IEnumerable<SortDefinition>? sorts) - získání stránkovaných a seřazených dat na základě expression, `TSort` je konkrétní `BaseSort<TEntity>`

- `Task<(IReadOnlyList<TProjection> Items, int Count)>` **GetPagedSortProjectedAsync**<TSort, TProjection>(...) - obdoba výše, ale s promítnutím (`Select`) na `TProjection`

- `Task<IEnumerable<TEntity>>` **GetFilteredSortForExportAsync**<TSort>(Expression<Func<TEntity, bool>>? filter, IEnumerable<SortDefinition>? sorts) - získání všech (nestránkovaných) seřazených záznamů pro export

- `Task<int>` **GetCountAsync**(Expression<Func<TEntity, bool>>? filter) - získání celkového počtu záznamů v tabulce na základě expression

- `Task<IEnumerable<TEntity>>` **GetFilteredAsync**(Expression<Func<TEntity, bool>>? filter) - získání všech záznamů v tabulce na základě expression

- `Task<IEnumerable<TEntity>>` **GetAllAsync**() - získání všech záznamů v tabulce

- **MarkAsModified**(TEntity entity) - explicitní označení entity jako `Modified` bez volání `Update` (např. při práci s detached entitou)

- `Task` **AddAsync**(TEntity entity) / **AddRangeAsync**(IEnumerable<TEntity> entities) - přidání záznamu/záznamů

- **Update**(TEntity entity) / **UpdateRange**(IEnumerable<TEntity> entities) - aktualizace záznamu/záznamů

- **Remove**(TEntity entity) / **RemoveRange**(IEnumerable<TEntity> entities) - smazání záznamu/záznamů (soft delete, viz [Databáze](../Databaze.md))

-----------------------
----------------------
Transakci potvrdím pomocí metody `Commit()`/`CommitAsync()` na `UnitOfWork`
jakéhokoliv repozitáře (metoda volá `SaveChanges`/`SaveChangesAsync` na celém
`DbContext`, tedy commit dělám pro celý `DbContext`, ne jen pro jeden
repozitář):

`clientRepository.UnitOfWork.Commit();`

nebo asynchronně:

`await clientRepository.UnitOfWork.CommitAsync(cancellationToken);`

**Update**
Auditní pole (`CreatedBy`/`CreatedAt`/`UpdatedBy`/`UpdatedAt`) i soft delete se
plní automaticky v `SaveChanges`/`SaveChangesAsync` v
`EntityFrameworkDbContextBase` (viz [Databáze](../Databaze.md)). Auditování
funguje podle stavu entity v EF Core change trackeru:

- `EntityState.Added` → nastaví se `CreatedBy` a `CreatedAt`.
- `EntityState.Modified` nebo `EntityState.Deleted` → nastaví se `UpdatedBy`
  a `UpdatedAt`. Soft delete se před uložením převede na `Modified` a nastaví
  `IsDeleted = true`.

Pokud je načtená entita sledovaná stejným `DbContext` a změní se její
property, EF Core její stav při detekci změn nastaví na `Modified`; není tedy
nutné kvůli auditním polím volat `Update()` ručně. Totéž platí pro změněné
entity v zanořené kolekci, pokud jsou správně připojené a sledované.

`Update()` (resp. `UpdateRange()`) použijte u detached entity/grafu, který
context nesleduje, nebo pokud potřebujete explicitně označit konkrétní entitu
jako změněnou i bez změny běžné property. To je relevantní i u zanořených
entit, které se mají auditovat jako aktualizované, ale jejich stav by jinak
zůstal `Unchanged`. Metoda `DbSet.Update()` ale označuje celý předaný graf jako
`Modified` (s výjimkami pro nové entity), takže EF může vygenerovat `UPDATE`
pro všechny mapované sloupce, i když se jejich hodnoty nezměnily. Nepoužívejte
ji proto plošně na všechny entity jen kvůli auditu; u sledovaných entit
preferujte běžnou změnu property a nechte EF změny detekovat.

Důsledek současné implementace: pokud chcete aktualizovat pouze auditní pole
entity, která nemá žádnou jinou změnu, musí být její stav explicitně nastaven
na `Modified` (např. přes `Update()` nebo `MarkAsModified()`), protože
`ProcessAuditFields()` zpracovává pouze stavy `Added`, `Modified` a `Deleted`.
Nejde o manuální interceptor – jde o standardní chování repository/change
trackeru a automatické zpracování v `EntityFrameworkDbContextBase`.

## Filtrování, řazení a stránkování

List endpointy používají `FilteredRequest<TFilter>`. Objekt obsahuje:

- `Filter` – API filtr konkrétní entity,
- `Offset` – počet záznamů, které se přeskočí,
- `Limit` – maximální počet vrácených záznamů; kladná hodnota stránkuje data,
- `Sort` – zkrácená forma pro jedno řazení,
- `Sorts` – seznam řazení ve formátu `field,direction`, například
  `Name,Asc` nebo `CreatedAt,Desc`.

`Sort` se při čtení převede na jeden prvek `Sorts`. Vlastnost
`ParsedSorts` následně textové hodnoty rozparsuje na `SortDefinition`:

```csharp
public class FilteredRequest<ClientFilter>
{
    public int Offset { get; set; }
    public int Limit { get; set; }
    public ClientFilter? Filter { get; set; }
    public List<string>? Sorts { get; set; }
}
```

Příklad query stringu může vypadat takto:

```text
GET /v2/Client?Offset=0&Limit=20&Sorts=Name,Asc&Sorts=CreatedAt,Desc&Filter.SearchField=acme
```

Konkrétní způsob serializace v klientovi se může lišit, ale výsledkem musí být
`FilteredRequest<TFilter>` se seznamem `Sorts`.

### Filtrování přes Specification

`ReadService.GetListAsync` provádí tento tok:

1. Pokud `request.Filter` není vyplněný, vytvoří prázdný filtr.
2. `ITypeMapper` namapuje `TFilter` na `TSpecification`.
3. `TSpecification.SatisfiedBy()` vytvoří
   `Expression<Func<TEntity, bool>>`.
4. Repository expression aplikuje na dotaz a vrátí data spolu s celkovým
   počtem záznamů před stránkováním.

Například `ClientSpecification` kombinuje `SearchField` přes OR nad poli
`Name`, `TradeId` a `Representative`. Ostatní filtry (`Name`, `TradeId`,
`VatId` atd.) se také převedou na podmínky nad `ClientEntity`. Více hodnot
jednoho filtru se skládá pomocí `BuildOrSpecification`, zatímco jednotlivé
části specifikace se kombinují podle implementace specifikace.

### Řazení přes `BaseSort<TEntity>`

Každý typ entity má vlastní sort provider, například `ClientSort`:

```csharp
public class ClientSort : BaseSort<ClientEntity>
{
    public override IReadOnlyList<SortClause<ClientEntity>> DefaultSorts =>
    [
        new(x => x.Name, SortDirection.Desc),
    ];
}
```

Repository vytvoří instanci `TSort` a pro každé požadované `SortDefinition`
vygeneruje výraz přes `BaseSort.GetSortingExpression`. Název property se hledá
case-insensitive a podporuje i vnořenou cestu, například
`Address.City.Name`. Neexistující nebo prázdná property vyvolá
`InvalidSortPropertyException`.

Požadované řazení se použije před defaultním řazením. Defaultní řazení se
proto připojí jako další `ThenBy`/`ThenByDescending` a poskytuje stabilní
tie-breaker pro záznamy se stejnou hodnotou požadovaného pole. Pokud klient
žádné řazení nezašle, použijí se pouze `DefaultSorts`.

Směr `Asc`/`Desc` je case-insensitive. Pokud směr chybí nebo není rozpoznán,
`FilteredRequest.ParsedSorts` použije `Asc`.

### Repository dotaz

Po vytvoření filtru a řazení repository:

1. spočítá `CountAsync()` nad filtrovaným dotazem,
2. aplikuje řazení,
3. aplikuje `Skip(offset)` a `Take(limit)`,
4. načte stránku dat.

`ReadService` výsledek vrací jako `PagedData<TListItem>` s položkami v
`Data` a celkovým počtem v `Total`. U projekcí se používá
`GetPagedSortProjectedAsync`, kde se `Select(selector)` aplikuje až po
řazení a stránkování.

```csharp
var specification = typeMapper.Map<ClientSpecification>(request.Filter) ?? new();
var filter = specification.SatisfiedBy();

var (items, count) = await clientRepository.GetPagedSortAsync<ClientSort>(
    request.Offset,
    request.Limit,
    filter,
    request.ParsedSorts);
```

### Přímý expression filtr

Do repository lze vložit i libovolnou expression, která se váže k entitě,
kterou repository vrací:

``` csharp
public async Task<ClientListData> GetClientListAsync(Verisoft.Core.Contracts.FilteredRequest<ClientFilter> request)
{
    var clientSpecification = typeMapper.Map<ClientSpecification>(request.Filter) ?? new();
    var filter = clientSpecification.SatisfiedBy();
    var totalItems = await clientRepository.GetCountAsync(filter);
    var items = await clientRepository.GetPagedSortAsync<ClientSort>(
        request.Offset,
        request.Limit,
        filter,
        request.ParsedSorts);
    var mappedClientData = typeMapper.Map<ClientListItem>(items);

    return new ClientListData()
    {
        Total = totalItems,
        Data = mappedClientData,
    };
}
```
Filtr pomocí TypeMapper přemapuji na Specification nad kterou následně zavolám SatisfiedBy() metodu, která mi vrátí expression.
Příklad Specification:


``` csharp
public class ClientSpecification : ISpecification<ClientEntity>
{
    public string[] Name { get; set; }

    public string[] TradeId { get; set; }

    public string[] VatId { get; set; }

    public string[] Representative { get; set; }

    public string[] TradeRegisterEntry { get; set; }

    public string[] AccountingSystemClientId { get; set; }

    public string[] CompanyActivity { get; set; }

    public Countries[] TaxDomicile { get; set; }

    public string SearchField { get; set; }

    public Expression<Func<ClientEntity, bool>> SatisfiedBy()
    {
        Specification<ClientEntity> specification = new TrueSpecification<ClientEntity>();
        if (SearchField.HasValue<string>())
        {
            var nameSpecification = new DirectSpecification<ClientEntity>(c => c.Name.Contains(SearchField));
            var tradeIdSpecification = new DirectSpecification<ClientEntity>(c => c.TradeId.Contains(SearchField));
            var representativeSpecification = new DirectSpecification<ClientEntity>(c => c.Representative.Contains(SearchField));
            specification = SpecificationExtender.OrAll(nameSpecification, tradeIdSpecification, representativeSpecification);
        }

        if (Name.HasValue())
        {
            specification = specification.BuildOrSpecification(Name, searchValue => (entity) => entity.Name.Contains(searchValue));
        }

        if (TradeId.HasValue())
        {
            specification = specification.BuildOrSpecification(TradeId, searchValue => (entity) => entity.TradeId.Contains(searchValue));
        }

        if (VatId.HasValue())
        {
            specification = specification.BuildOrSpecification(VatId, searchValue => (entity) => entity.VatId.Contains(searchValue));
        }

        if (Representative.HasValue())
        {
            specification = specification.BuildOrSpecification(Representative, searchValue => (entity) => entity.Representative.Contains(searchValue));
        }

        if (TradeRegisterEntry.HasValue())
        {
            specification = specification.BuildOrSpecification(TradeRegisterEntry, searchValue => (entity) => entity.TradeRegisterEntry.Contains(searchValue));
        }

        if (AccountingSystemClientId.HasValue())
        {
            specification = specification.BuildOrSpecification(AccountingSystemClientId, searchValue => (entity) => entity.AccountingSystemClientId.Contains(searchValue));
        }

        if (CompanyActivity.HasValue())
        {
            specification = specification.BuildOrSpecification(CompanyActivity, searchValue => (entity) => entity.CompanyActivity.Contains(searchValue));
        }

        if (TaxDomicile.HasValue())
        {
            specification = specification.BuildOrSpecification(TaxDomicile, searchValue => (entity) => entity.TaxDomicile == searchValue);
        }

        return specification.SatisfiedBy();
    }
}
```
