#Repository pattern

- Pro přístup k DbContextu používáme repozitáře
- Každá entita má vlastní repozitář
- Interface je implementací `Verisoft.Core.Common.Store.IRepository<TEntity, TKey>`
- Třída dědí od `Verisoft.Core.Data.EntityFramework.Repositories.EntityFrameworkRepositoryBase<TEntity, TKey>`


``` csharp
public interface IClientRepository : IRepository<ClientEntity, int>
{
}
```

``` csharp
public class ClientRepository(IDemoApiDbContext unitOfWork, IUserContext userContext, ILogger<ClientRepository> logger)
    : EntityFrameworkRepositoryBase<ClientEntity, int>(unitOfWork, userContext, logger), IClientRepository
{
    protected IDemoApiDbContext Context => (IDemoApiDbContext)UnitOfWork;

    protected override DbSet<ClientEntity> GetDbSet()
    {
        return Context.Client;
    }
}
```
- je potřeba nastavit DbSet v override metody GetDbSet()
- je potřeba nastavit Context z UnitOfWork, který se castne na interface DbContextu

registrace probíhá standardně v InstallExtensions 


``` csharp
private static void RegisterRepositories(IServiceCollection serviceCollection)
{
    serviceCollection.TryAddScoped<IClientRepository, ClientRepository>();
}
```