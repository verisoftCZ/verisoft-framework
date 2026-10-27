namespace Verisoft.Core.Data.EntityFramework.Test.Walkers;

[TestFixture]
public class CascadeWalkerSetNullTests : CascadeWalkerTestBase
{
    [Test]
    public async Task SoftDelete_ShouldSetChildFkToNull_WhenSetNullBehavior()
    {
        var parent = await SeedParentAsync();
        var child = await SeedSetNullChildAsync(parent.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        var savedChild = await Context.SetNullChildren
            .IgnoreQueryFilters()
            .FirstAsync(c => c.Id == child.Id);

        Assert.That(savedChild.ParentId, Is.Null);
    }

    [Test]
    public async Task SoftDelete_ShouldSetFkToNull_ForMultipleSetNullChildren()
    {
        var parent = await SeedParentAsync();
        var child1 = await SeedSetNullChildAsync(parent.Id);
        var child2 = await SeedSetNullChildAsync(parent.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        var saved1 = await Context.SetNullChildren.IgnoreQueryFilters().FirstAsync(c => c.Id == child1.Id);
        var saved2 = await Context.SetNullChildren.IgnoreQueryFilters().FirstAsync(c => c.Id == child2.Id);

        Assert.That(saved1.ParentId, Is.Null);
        Assert.That(saved2.ParentId, Is.Null);
    }

    [Test]
    public async Task SoftDelete_ShouldNotAffectSetNullChild_WhenFkAlreadyClearedInMemory()
    {
        var parent = await SeedParentAsync();
        var child = await SeedSetNullChildAsync(parent.Id);

        // Caller already cleared FK in memory before requesting delete.
        child.ParentId = null;
        child.Parent = null;

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        var savedChild = await Context.SetNullChildren.IgnoreQueryFilters().FirstAsync(c => c.Id == child.Id);

        // FK was already null in memory; walker must not attempt a double-null write.
        Assert.That(savedChild.ParentId, Is.Null);
        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldNotSoftDeleteSetNullChild_WhenParentDeleted()
    {
        // SetNull children are NOT soft-deleted by the walker – only their FK is cleared.
        var parent = await SeedParentAsync();
        var child = await SeedSetNullChildAsync(parent.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        Assert.That(await GetIsDeletedAsync<SetNullChildEntity>(child.Id), Is.False);
    }

    [Test]
    public async Task SoftDelete_ShouldHandleMixedBehaviors_WithoutErrors()
    {
        // Parent has both SetNull and Cascade children.
        var parent = await SeedParentAsync();
        var cascadeChild = await SeedCascadeChildAsync(parent.Id);
        var setNullChild = await SeedSetNullChildAsync(parent.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        Assert.That(await GetIsDeletedAsync<CascadeChildEntity>(cascadeChild.Id), Is.True);
        Assert.That(await GetIsDeletedAsync<SetNullChildEntity>(setNullChild.Id), Is.False);

        var savedSetNullChild = await Context.SetNullChildren.IgnoreQueryFilters().FirstAsync(c => c.Id == setNullChild.Id);
        Assert.That(savedSetNullChild.ParentId, Is.Null);
    }
}
