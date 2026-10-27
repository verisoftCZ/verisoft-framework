# MassTransit a RabbitMQ
https://masstransit.io/documentation/concepts
Stručné vysvětlení:
Jedná se o mezislužbu, která je většinou umístěná mezi dvěma komponentami, například mezi dvěma servery. Slouží k tomu, aby v případě výpadku jednoho ze serverů zprávy (dotazy), které na něj směřovaly, nezmizely, ale zůstaly zachovány – konkrétně například v RabbitMQ. Po obnovení spojení jsou pak zprávy zpracovány.

Ukázka je implementovaná pod masstransitcontrollerem v demoAPI

Docker:
`docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management`

Docker compose:
```yaml
  rabbitmq:
    image: rabbitmq:4-management
    container_name: rabbitmq
    restart: always
    ports:
      - "5672:5672"     
      - "15672:15672"   
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
```

---
# Implementace:
Potřeba
`Verisoft.Core.MassTransit` - [Nuget](https://www.nuget.org/packages/Verisoft.Core.MassTransit)

## Základ
```cs
    private static void RegisterMassTransit(IServiceCollection services, IConfiguration configuration)
    {
        services.AddVerisoftServiceBus<Program>(
            configuration,
            options
                =>
            {
                 // Sem options
            });
    }
```
Tímhle nastavíte fungování masstransitu, 
```json
  "MassTransit": {
    "Server": {
      "Url": "rabbitmq://localhost", (pro docker: rabbitmq://rabbitmq)
      "UserName": "guest",
      "Password": "guest",
      "UseSsl": false
    }
  }
```
nezapoměnout přidat do .appsettings
(pokud běží v dockeru, tak připojeni je: `MassTransit__Server__Url=rabbitmq://rabbitmq`)

---
## Přidání service na posílání commandu
Jedná se jen o zapouzdření do našeho objektu.
```cs
public class RabbitSenderService(IPublishEndpoint publishEndpoint) : IRabbitSenderService
    {
        public async Task Send<TPayload>(TPayload command)
            where TPayload : class, IRabbitPayload
        {
            await publishEndpoint.Publish(command);
        }
    }
```
    
```cs
public interface IRabbitSenderService
    {
        Task Send<TPayload>(TPayload command)
            where TPayload : class, IRabbitPayload;
    }
```
    
```cs
public interface IRabbitPayload {}
```

`serviceCollection.TryAddScoped<IRabbitSenderService, RabbitSenderService>();`

---
## Nastavení
### Consumer
Pro nastavení consumera (ten kdo přijmá) je nutné napasat do options

```cs
 options.ConfigureConsumer<CreateDocumentConsumer>()
    .MaxRetries(3)
    .ConcurrencyLimit(1);
```
---
### Saga - [Saga State Machines](https://masstransit.io/documentation/patterns/saga/state-machine#saga-state-machines)

Pro nastavení sagy je nutné napsat do options:
```cs
options.ConfigureSaga<DocumentSaga, DocumentSagaState>().MaxRetries(3);
```
Pokud implementujeme sagu, tak je nutné někde postup uchovávat. Defaultně se to dělá přes uchování v RAM, ovšem je možné tohle přehodit přes EF na databázi.
Pro uchování v RAM (nastaveno defaultně, není nutné definovat):
```cs
options.SetSagaRepository(c =>
{
    c.SetSagaRepositoryProvider(new InMemorySagaRepositoryRegistrationProvider());
});
```
### Pro uchování přes EntityFramework:
https://masstransit.io/documentation/configuration/persistence/entity-framework
Bude to fungovat tak, že pokud se Saga dostane do nějakého stavu, potom se zapíše do DB, dokuď stav není hotový a nezavolá se metoda `.Finalize()`

Rozdíl mezi
`ConcurrencyMode.Pessimistic` / `ConcurrencyMode.Optimistic;`

|Režim  |Vhodné  | Výhody |Nevýhody  |
|--|--|--|--|
| Optimistic |Nízká kontaminace změn, většina scénářů  | Rychlost, nízké zámky | Nutnost retry při konfliktu |
| Pessimistic |Vysoká šance na konflikty při současných zápisech  |Žádné konflikty |  Lze zablokovat ostatní, snížený výkon|

Při `optimistic` je nutné přidat do sagaStates tohle:
`public uint RowVersion { get; set; }`


#### Mssql
```cs
options.SetSagaRepository(c =>
{
    c.SetEntityFrameworkSagaRepositoryProvider(r =>
    {
        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
        r.AddDbContext<DbContext, DemoApiDbContext>((provider, builder) =>
        {
            var conn = configuration["Database:ConnectionString"];
            builder.UseSqlServer(conn, m =>
            {
                m.MigrationsAssembly(typeof(DemoApiDbContext).Assembly.GetName().Name);
                m.MigrationsHistoryTable($"__{nameof(DemoApiDbContext)}");
            });
        });
    });
});
```
#### PostgreSQL
```cs
options.SetSagaRepository(c =>
{
    c.SetEntityFrameworkSagaRepositoryProvider(r =>
    {
        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
        r.AddDbContext<DbContext, DemoApiDbContext>((provider, builder) =>
        {
            var conn = configuration["Database:ConnectionString"];
            builder.UseNpgsql(conn, m =>
            {
                m.MigrationsAssembly(typeof(DemoApiDbContext).Assembly.GetName().Name);
                m.MigrationsHistoryTable($"__{nameof(DemoApiDbContext)}");
            });
        });
        r.UsePostgres();
    });
});
```