#Užití
**do databáze se přistupuje výhradně přes repozitáře**

Repozitář poskytuje v základu většinu potřebných metod, které lze i overridnout.

- bool **ExistsAsync**(TKey id) - ověření existence entity dle jejího primárního klíče

- TEntity **GetAsync**(TKey id) - získání entity dle jejího primárního klíče

- IEnumerable<TEntity> **GetPagedAsync**(int offset, int limit, Expression<Func<TEntity, bool>> filter) - získání paged dat na základě expression

- IEnumerable<TEntity> **GetPagedSortAsync**<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true) - získání paged sorted dat na základě expression

- int **GetCountAsync**(Expression<Func<TEntity, bool>> filter) - získání celkového počtu záznamů v tabulce na základě expression

- IEnumerable<TEntity> **GetFilteredAsync**(Expression<Func<TEntity, bool>> filter) - získání všech záznamů v tabulce na základě expression

- IEnumerable<TEntity> **GetAll**()  - získání všech záznamů v tabulce

- **AddAsync**(TEntity entity) - přidání záznamu

- **UpdateAsync**(TEntity entity) - aktualizace záznamu

- **UpdateRangeAsync**(TEntity[] entities) - aktualizace kolekce záznamů

- **RemoveAsync**(TEntity entity) - smazání záznamu

- **RemoveRangeAsync**(TEntity[] entities) - smazání kolekce záznamů
-----------------------
----------------------
Transakci potvrdím pomocí metody Commit na UnitOfWork jakéhokoliv repozitáře (Commit dělám pro celý DbContext)

`clientRepository.UnitOfWork.Commit();`

**Update**
Při update včetně zanořené kolekce je potřeba zavolat metodu Update() na všech aktualizovaných entitách z důvodu naplnění auditních polích (do budoucna bude řešeno pomocí interceptoru, který by tuto nutnost měl odstranit)

**Get s expression**
Do parametru filter lze vložit libovolnou expression, která se váže k entitě, kterou repozitář vrací.
Pro jednodušší filtrování používáme Filter => Specification pattern jak je vidět v metodě níže.

``` csharp
public async Task<ClientListData> GetClientListAsync(Verisoft.Core.Contracts.FilteredRequest<ClientFilter> request)
{
    var clientSpecification = typeMapper.Map<ClientSpecification>(request.Filter);
    var filter = clientSpecification?.SatisfiedBy();
    var totalItems = await clientRepository.GetCountAsync(filter);
    var items = await clientRepository.GetPagedSortAsync<ClientSort>(
        request.Offset,
        request.Limit,
        filter,
        request.Sort);
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


