namespace Verisoft.Core.Data.EntityFramework.Test.Walkers;

[TestFixture]
public class CascadeWalkerCascadeTests : CascadeWalkerTestBase
{
    [Test]
    public async Task SoftDelete_ShouldSoftDeleteParent_WhenRemoved()
    {
        var parent = await SeedParentAsync();

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldCascade_ToCascadeChild()
    {
        var parent = await SeedParentAsync();
        var child = await SeedCascadeChildAsync(parent.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        Assert.That(await GetIsDeletedAsync<CascadeChildEntity>(child.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldCascadeRecursively_ToGrandChild()
    {
        var parent = await SeedParentAsync();
        var child = await SeedCascadeChildAsync(parent.Id);
        var grandChild = await SeedGrandChildAsync(child.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        Assert.That(await GetIsDeletedAsync<CascadeChildEntity>(child.Id), Is.True);
        Assert.That(await GetIsDeletedAsync<GrandChildEntity>(grandChild.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldCascade_ToMultipleChildren()
    {
        var parent = await SeedParentAsync();
        var child1 = await SeedCascadeChildAsync(parent.Id);
        var child2 = await SeedCascadeChildAsync(parent.Id);

        Context.Remove(parent);
        await Context.SaveChangesAsync();

        Assert.That(await GetIsDeletedAsync<CascadeChildEntity>(child1.Id), Is.True);
        Assert.That(await GetIsDeletedAsync<CascadeChildEntity>(child2.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldNotCascade_ToAlreadySoftDeletedChild()
    {
        var parent = await SeedParentAsync();
        var child = await SeedCascadeChildAsync(parent.Id);

        // Pre-delete the child so IsDeleted=true is already in DB.
        child.IsDeleted = true;
        await Context.SaveChangesAsync();

        // The parent should be soft-deletable; walker skips already-deleted children.
        Context.Remove(parent);
        Assert.DoesNotThrowAsync(async () => await Context.SaveChangesAsync());

        Assert.That(await GetIsDeletedAsync<ParentEntity>(parent.Id), Is.True);
    }

    [Test]
    public async Task SoftDelete_ShouldNotProcessSameEntity_Twice()
    {
        // Cycle guard: entity appearing in its own cascade should not loop.
        var parent = await SeedParentAsync();
        await SeedCascadeChildAsync(parent.Id);

        Context.Remove(parent);

        // Should complete without StackOverflowException or duplicate processing.
        Assert.DoesNotThrowAsync(async () => await Context.SaveChangesAsync());
    }
}
