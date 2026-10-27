using Microsoft.Extensions.Logging.Abstractions;
using Verisoft.Core.Data.EntityFramework.Walkers;

namespace Verisoft.Core.Data.EntityFramework.Test.Walkers.TestInfrastructure;

public abstract class CascadeWalkerTestBase
{
    protected TestDbContext Context { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        // Each test gets its own isolated InMemory database.
        var dbName = $"CascadeWalkerTest_{TestContext.CurrentContext.Test.FullName}_{Guid.NewGuid():N}";
        var userContext = new StubUserContext();
        var walkerFactory = new WalkerFactory(NullLogger<CascadeWalker>.Instance);

        Context = new TestDbContext(dbName, userContext, walkerFactory, TimeProvider.System);
        Context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }

    protected async Task<ParentEntity> SeedParentAsync(string name = "Parent")
    {
        var parent = new ParentEntity { Name = name };
        Context.Parents.Add(parent);
        await Context.SaveChangesAsync();
        return parent;
    }

    protected async Task<CascadeChildEntity> SeedCascadeChildAsync(int parentId)
    {
        var child = new CascadeChildEntity { ParentId = parentId };
        Context.CascadeChildren.Add(child);
        await Context.SaveChangesAsync();
        return child;
    }

    protected async Task<GrandChildEntity> SeedGrandChildAsync(int cascadeChildId)
    {
        var grandChild = new GrandChildEntity { CascadeChildId = cascadeChildId };
        Context.GrandChildren.Add(grandChild);
        await Context.SaveChangesAsync();
        return grandChild;
    }

    protected async Task<RestrictChildEntity> SeedRestrictChildAsync(int parentId)
    {
        var child = new RestrictChildEntity { ParentId = parentId };
        Context.RestrictChildren.Add(child);
        await Context.SaveChangesAsync();
        return child;
    }

    protected async Task<SetNullChildEntity> SeedSetNullChildAsync(int parentId)
    {
        var child = new SetNullChildEntity { ParentId = parentId };
        Context.SetNullChildren.Add(child);
        await Context.SaveChangesAsync();
        return child;
    }

    protected async Task<bool> GetIsDeletedAsync<T>(int id)
        where T : class, Verisoft.Core.Common.Entities.IBaseEntity
    {
        var entity = await Context.Set<T>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => ((Verisoft.Core.Common.Entities.IEntity)e).Id.Equals(id));

        return entity?.IsDeleted ?? throw new InvalidOperationException($"Entity {typeof(T).Name} id={id} not found.");
    }
}
