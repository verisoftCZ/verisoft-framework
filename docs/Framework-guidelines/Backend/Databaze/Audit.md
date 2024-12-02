#Obecné

- probíhá automaticky při Update, UpdateRange, Remove a RemoveRange (delete je soft, takže technicky probíhá update)
- porovnává se stará a nová hodnota každé property
-- ignorují se property typu entita (na entitě Order mám property Customer, při update Order neaudituji Customera)
-- do auditu jsou zahrnuty vnořené kolekce, kde se neporovnávají celé jednotlivé elementy, ale pouze jejich Idčka (Na entitě Customer mám kolekci Orders o dvou elementech s Ids 5 a 10. Připojím nový Order s Id 15 a odpojím order s Id 5. Do auditu se propíše| PropertyName: Orders | OldValue: 5;10 | NewValue: 10;15 |.)
- audit má jen dvě tabulky EntityAudit (hlavička) a EntityAuditDetail (záznamy změněných propert) ve schématu "audit", pro jedno API. Data zůstávají v databázi daného API kvůli bezpečnosti.

#Nastavení
- DbContext v API musí dědit od `Verisoft.Core.Data.EntityFramework.EntityFrameworkDbContextBase`
-- kvůli registraci DbSetů auditních tabulek a jejich konfiguraci
-- mohu také přenastavit DefaultSchema, které je přednastavené na "dbo" => fallback pokud se nepodaří vyčíst z konfigurace entity
- Repozitář musí dědit od `Verisoft.Core.Data.EntityFramework.Repositories.EntityFrameworkRepositoryBase​` a být implementací `Verisoft.Core.Common.Store.IRepository`
-- je potřeba injectnout ILogger<RepositoryClass>
-- možnost nastavit chování, že v případě selhání auditu nedojde pouze k zalogování chyby auditu, ale i k throw => defaultně false
-- možnost nastavit property, které se ignorují při porovnávání (case sensitive, doporučuji používat `nameof(ClientEntity.VatId)` => defaultně null

``` csharp
public abstract class BaseRepository<TEntity, TKey>(
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    ILogger<BaseRepository<TEntity, TKey>> logger,
    bool throwIfAuditFails = false,
    IEnumerable<string> ignoredProperties = null) : EntityFrameworkRepositoryBase<TEntity, TKey>(unitOfWork, userContext, logger, throwIfAuditFails, ignoredProperties)
    where TEntity : class, IBaseEntity<TKey>
{
    protected IDemoApiDbContext Context => (IDemoApiDbContext)UnitOfWork;
}
```

``` csharp
public class ClientRepository(
    IDemoApiDbContext unitOfWork,
    IUserContext userContext,
    ILogger<ClientRepository> logger)
    : BaseRepository<ClientEntity, int>(unitOfWork, userContext, logger, true, [nameof(ClientEntity.VatId), nameof(ClientEntity.CompanyActivity)]), IClientRepository
{
    protected override DbSet<ClientEntity> GetDbSet()
    {
        return Context.Client;
    }
}
```

#Get

- filtr


``` csharp
namespace Verisoft.Core.Contracts.Audit;

public class EntityAuditFilter
{
    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime? ChangedAtFrom { get; set; }

    public DateTime? ChangedAtTo { get; set; }

    public string[] ChangedAttributes { get; set; } = [];

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}
```

- specification
-- pro detail `Verisoft.Core.Common.Audit.EntityAuditDetailSpecification` 
-- pro hlavičku `Verisoft.Core.Common.Audit.EntityAuditSpecification` 
- mapovací extension metody ve `Verisoft.Core.Common.Audit.MappingExtensions`
-- filtr => specification hlavičky
-- filtr => specification detailu
-- audit entita => audit contract vč. zanořených detailů (i pro kolekci)
-- audit detail entita => audit detail contract vč. vnořené hlavičky (i pro kolekci)
-- audit detail entita => flattened audit detail (i pro kolekci)

 
``` csharp
public async Task<PagedData<EntityAudit>> GetAudit(FilteredRequest<EntityAuditFilter> request)
{
    var specification = request.Filter.ToSpecification();
    var filter = specification?.SatisfiedBy();
    var count = await clientRepository.GetEntityAuditCountAsync(filter);
    var audit = await clientRepository.GetPagedEntityAuditAsync<EntityAuditSort>(request.Offset, request.Limit, filter, request.Sort);

    return new PagedData<EntityAudit>
    {
        Data = audit.ToContract(),
        Total = count,
    };
}
```

- metody

``` csharp
Task<IEnumerable<EntityAuditEntity>> GetPagedEntityAuditAsync<TSort>(int offset, int limit, Expression<Func<EntityAuditEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true)
where TSort : BaseSort<EntityAuditEntity>;

Task<IEnumerable<EntityAuditDetailEntity>> GetPagedEntityAuditDetailAsync<TSort>(int offset, int limit, Expression<Func<EntityAuditDetailEntity, bool>> filter, SortDefinition sort, bool isAscendingSortByDefault = true) 
where TSort : BaseSort<EntityAuditDetailEntity>;

Task<int> GetEntityAuditCountAsync(Expression<Func<EntityAuditEntity, bool>> filter);

Task<int> GetEntityAuditDetailCountAsync(Expression<Func<EntityAuditDetailEntity, bool>> filter);
```





