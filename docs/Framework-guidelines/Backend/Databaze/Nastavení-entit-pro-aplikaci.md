#Nastavení entit pro aplikaci

Entity dědí od Verisoft.Core.Common.Entitie.BaseEntity<TKey>
- zajišťuje definici PK property s názvem Id
- zajišťuje soft delete
- zajišťuje plnění auditních polí (CreatedBy, CreatedAt, UpdatedBy, UpdatedAt)


``` csharp
public class ClientEntity : BaseEntity<int>
{
    public string Name { get; set; }

    public string Representative { get; set; }
}
```


Ve výjimečných případech lze použít dědění pouze od Verisoft.Core.Common.Entities.Entity<TKey>
- zajišťuje definici PK property s názvem Id
- použitelné v případě m : n vazby vůči entitě mimo databázi API

``` csharp
public class ClientCityEntity : Entity<int>
{
    public int ClientId { get; set; } // Db FK

    public int CityId { get; set; } // falešný FK
}
```