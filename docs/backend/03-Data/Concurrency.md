# Concurrency (zamykání záznamů)

> Tato funkcionalita v předchozí verzi wiki nebyla popsána vůbec. Dokument
> vznikl na základě analýzy kódu `Verisoft.Core.AspNet.Concurency`.

Modul `Verisoft.Core.AspNet.Concurency` řeší souběžný přístup k entitám ve
dvou rovinách:

## Optimistic concurrency

> Současná implementace je vázaná na SQL Server – vlastnost `Version` je
> mapovaná na sloupec typu `rowversion` (`IsRowVersion()`), který je
> specifický pro SQL Server. Pro jiný databázový provider je potřeba mapování
> upravit (jiný mechanismus concurrency tokenu).

Řeší se automaticky pro všechny entity implementující
`IOptimisticLockable` (vlastnost `Version`, řádková verze/`rowversion`, viz
[Databáze](Databaze.md)). `OptimisticConcurrencyInterceptor`
(`[RegisterInterceptor]`, tedy automaticky registrovaný přes stejný
mechanismus jako ostatní EF interceptory, viz [Databáze](Databaze.md)) při
`SavingChanges`/`SavingChangesAsync` nastaví `OriginalValue` sloupce
`Version` – EF Core tak při konfliktní změně (jiný uživatel mezitím záznam
změnil) vyhodí `DbUpdateConcurrencyException`.

## Pesimistic concurrency (zamykání záznamu)

Pro entity implementující `IPesimisticLockable` (vlastnosti `LockedAt`,
`LockedBy`) modul poskytuje explicitní zamykání záznamu na dobu editace:

- `ILockingManager.Lock<TEntity>(int id)` – nastaví `LockedAt`/`LockedBy`
  (aktuální uživatel z `IUserContext`) a commitne.
- `ILockingManager.Unlock<TEntity>(int id)` – vynuluje `LockedAt`/`LockedBy`.
- `PesimisticConcurencyIterceptor` (`[RegisterInterceptor]`) při uložení
  změny ověří, že entitu upravuje stejný uživatel, který ji zamkl – pokud ne,
  vyhodí `DbUpdateConcurrencyException`.

### Registrace

```csharp
services.AddConcurency<IDemoApiDbContext>();
```

Registruje `ILockingManager`/`LockingManager<TDbContext>` a exception
handler `ConcurrencyExceptionHandler`.

### Endpointy

```csharp
app.UseConcurency();
```

Pro každou entitu implementující `IPesimisticLockable` (detekováno reflexí
přes všechny načtené assembly) automaticky namapuje minimal-API endpointy:

- `POST {EntityName}/{id}/lock`
- `DELETE {EntityName}/{id}/lock`

Oba endpointy vyžadují autorizaci (`RequireAuthorization()`) a vrací
`200 OK` / `401 Unauthorized` / `403 Forbidden` / `422 Unprocessable Entity`.
