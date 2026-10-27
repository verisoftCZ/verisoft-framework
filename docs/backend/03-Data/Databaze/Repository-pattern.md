# Repository pattern

- Pro přístup k `DbContext` používáme repozitáře, přímý přístup k `DbContext`
  z aplikační vrstvy se nepoužívá.
- Každá entita (typicky agregát) má vlastní repozitář.
- Rozhraní repozitáře dědí z `Verisoft.Core.Common.Store.IRepository<TEntity, TKey>`
  (`Verisoft.Core.Common`), které definuje standardní CRUD/query metody
  (`GetAsync`, `GetFilteredAsync`, `GetPagedSortAsync<TSort>`, `AddAsync`,
  `Update`, `Remove`, `MarkAsModified`, ...).
- Implementace dědí buď přímo od
  `Verisoft.Core.Data.EntityFramework.Repositories.EntityFrameworkRepositoryBase<TEntity, TKey>`,
  nebo (pro entity, u kterých je zapnutý EF Core temporal history, viz
  [Databáze](../Databaze.md)) od
  `Verisoft.Core.Data.EntityFramework.Repositories.HistoryRepositoryBase<TEntity, TKey>`,
  která z `EntityFrameworkRepositoryBase` dědí a přidává práci s historií.

``` csharp
public interface IClientRepository : IRepository<ClientEntity, int>
{
    Task<ClientEntity?> FindByTradeIdAsync(string tradeId, CancellationToken cancellationToken);

    Task<ClientEntity?> FindByVatIdAsync(string vatId, CancellationToken cancellationToken);
}
```

``` csharp
public class ClientRepository(IDemoApiDbContext unitOfWork)
    : HistoryRepositoryBase<ClientEntity, int>(unitOfWork), IClientRepository
{
    public async Task<ClientEntity?> FindByTradeIdAsync(string tradeId, CancellationToken cancellationToken)
    {
        return await GetDbSet()
            .FirstOrDefaultAsync(c => c.TradeId == tradeId, cancellationToken);
    }

    public async Task<ClientEntity?> FindByVatIdAsync(string vatId, CancellationToken cancellationToken)
    {
        return await GetDbSet()
            .FirstOrDefaultAsync(c => c.VatId == vatId, cancellationToken);
    }
}
```

- Repozitář dostává v konstruktoru `IUnitOfWork` (v praxi konkrétní
  `IDemoApiDbContext`/`DbContext`), který je dostupný přes vlastnost
  `UnitOfWork` z `IRepository<TEntity, TKey>`.
  `UnitOfWork.Set<TEntity>()`.
- Pro vlastní chování při čtení (např. `Include`, filtrování dle role) lze
  dále overridnout `GetQueryable()`, `GetQueryableForDetail()` nebo
  `GetQueryableForExport()`.

Registrace probíhá standardně v `InstallExtensions` daného modulu:

``` csharp
private static void RegisterRepositories(IServiceCollection serviceCollection)
{
    serviceCollection.TryAddScoped<IClientRepository, ClientRepository>();
}
```