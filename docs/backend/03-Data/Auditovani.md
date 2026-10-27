# Auditování

Auditní pole entit (`CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`) se u
všech entit implementujících `IBaseEntity` (dědících z `BaseEntity<TKey>`)
plní automaticky v `EntityFrameworkDbContextBase.SaveChanges`/`SaveChangesAsync`
(`ProcessAuditFields`) – není potřeba psát vlastní auditní logiku ani
interceptor, viz [Databáze](Databaze.md). Uživatelské jméno se získává přes
`IUserContext.GetUserName()`, čas přes injektovaný `TimeProvider`.

Auditní záznamy nesmí obsahovat tajemství ani citlivé hodnoty, které aplikace
nemá ukládat.

Auditování změn souborů patří do vrstvy Blob Storage a má používat stejný
identifikátor operace jako aplikační logování. Před implementací ověřte, zda
daný modul už auditní službu poskytuje, a nevytvářejte paralelní mechanismus.
