using Verisoft.Core.Data.EntityFramework.Walkers;

namespace Verisoft.Core.Data.EntityFramework.Test.Walkers;

[TestFixture]
public class CascadeWalkerRestrictTests : CascadeWalkerTestBase
{
    [Test]
    public async Task SoftDelete_ShouldThrowEntityDeleteBlockedException_WhenActiveRestrictChildExists()
    {
        var parent = await SeedParentAsync();
        var child = await SeedRestrictChildAsync(parent.Id);

        // Keep persisted FK reference active while avoiding in-memory relationship fixup.
        Context.Entry(child).State = EntityState.Detached;

        Context.Remove(parent);

        Assert.ThrowsAsync<EntityDeleteBlockedException>(async () => await Context.SaveChangesAsync());
    }

    [Test]
    public async Task SoftDelete_ShouldIncludeViolatingTypeInException_WhenRestrictChildExists()
    {
        var parent = await SeedParentAsync();
        var child = await SeedRestrictChildAsync(parent.Id);

        // Keep persisted FK reference active while avoiding in-memory relationship fixup.
        Context.Entry(child).State = EntityState.Detached;

        Context.Remove(parent);

        var ex = Assert.ThrowsAsync<EntityDeleteBlockedException>(async () => await Context.SaveChangesAsync());
        Assert.That(ex!.Message, Does.Contain(nameof(RestrictChildEntity)));
    }

    [Test]
    public async Task SoftDelete_ShouldSucceed_WhenRestrictChildIsTrackedAndRelationshipFixupClearsFk()
    {
        // Documents EF fixup behavior:
        // with a tracked optional dependent, removing principal can clear FK in-memory.
        // The walker should respect current tracked state and not block delete.
        var parent = await SeedParentAsync();
        var child = await SeedRestrictChildAsync(parent.Id);

        Context.Remove(parent);

        Assert.DoesNotThrowAsync(async () => await Context.SaveChangesAsync());

        var savedChild = await Context.RestrictChildren
            .IgnoreQueryFilters()
            .FirstAsync(c => c.Id == child.Id);

        Assert.That(savedChild.ParentId, Is.Null);
        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldSucceed_WhenRestrictChildFkClearedInMemory()
    {
        // Reproduces the original bug:
        // The child was persisted in the DB referencing the parent.
        // Before deleting the parent the caller clears both the FK and navigation in memory.
        // The walker must respect the current tracked value (null) and not find a violation.
        var parent = await SeedParentAsync();
        var child = await SeedRestrictChildAsync(parent.Id);

        // Simulate what the application code does: clear FK + nav before requesting delete.
        child.ParentId = null;
        child.Parent = null;

        Context.Remove(parent);

        Assert.DoesNotThrowAsync(async () => await Context.SaveChangesAsync());
        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldSucceed_WhenRestrictChildIsAlreadySoftDeleted()
    {
        var parent = await SeedParentAsync();
        var child = await SeedRestrictChildAsync(parent.Id);

        // Mark the child as already soft-deleted.
        child.IsDeleted = true;
        await Context.SaveChangesAsync();

        Context.Remove(parent);

        Assert.DoesNotThrowAsync(async () => await Context.SaveChangesAsync());
        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldSucceed_WhenParentHasNoRestrictChildren()
    {
        var parent = await SeedParentAsync();

        Context.Remove(parent);

        Assert.DoesNotThrowAsync(async () => await Context.SaveChangesAsync());
        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldThrow_WhenOneOfMultipleRestrictChildrenIsActive()
    {
        var parent = await SeedParentAsync();
        var activeChild = await SeedRestrictChildAsync(parent.Id);
        var softDeletedChild = await SeedRestrictChildAsync(parent.Id);

        softDeletedChild.IsDeleted = true;
        await Context.SaveChangesAsync();

        // Active child (activeChild) still points to parent → must block.
        _ = activeChild;
        Context.Entry(activeChild).State = EntityState.Detached;
        Context.Entry(softDeletedChild).State = EntityState.Detached;
        Context.Remove(parent);

        Assert.ThrowsAsync<EntityDeleteBlockedException>(async () => await Context.SaveChangesAsync());
    }
}
