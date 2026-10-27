# Audit (historie změn property)

> **Tato funkcionalita byla z frameworku odstraněna.** Tabulky `EntityAudit`
> a `EntityAuditDetail` (schéma `audit`) byly v Demo API zrušeny migrací
> `Remove_AuditTables` (viz
> `src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF/Migrations/20241223064018_Remove_AuditTables.cs`)
> a v aktuálním kódu neexistuje žádná třída `EntityAuditEntity`,
> `EntityAuditDetailEntity`, `EntityAuditFilter`, `EntityAuditSpecification`
> ani `Verisoft.Core.Common.Audit.MappingExtensions` popisované dříve v tomto
> dokumentu. `BaseRepository<TEntity, TKey>` v Demo API
> (`Verisoft.DemoApi.Data.EF.Repositories.BaseRepository`) je dnes jen tenký
> wrapper kolem `EntityFrameworkRepositoryBase<TEntity, TKey>` bez parametrů
> `throwIfAuditFails`/`ignoredProperties` a bez auditní logiky.
>
> Co framework **skutečně** poskytuje, je pouze automatické plnění
> auditních polí na entitě (`CreatedBy`/`CreatedAt`/`UpdatedBy`/`UpdatedAt`)
> v `EntityFrameworkDbContextBase.ProcessAuditFields`/`ApplyAuditInfo` – tedy
> hlavičkové informace o tom, kdo a kdy záznam naposledy změnil, ne historii
> jednotlivých hodnot property. Viz [Auditování](../Auditovani.md) a
> [Databáze](../Databaze.md).
>
> Pokud je potřeba sledovat historii hodnot jednotlivých property (ne jen
> kdo/kdy), lze místo vlastního audit mechanismu využít EF Core temporal
> tables (`ToTable(t => t.IsTemporal())`), které se v `ClientConfiguration`
> již používají – viz [Db konfigurace](Db-konfigurace.md).
