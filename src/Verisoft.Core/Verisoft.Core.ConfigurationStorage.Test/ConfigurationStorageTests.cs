using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.ConfigurationStorage.Test;

[TestFixture]
public class ConfigurationStorageTests
{
    private SqliteConnection connection = null!;
    private ServiceProvider provider = null!;
    private StubUserContext identity = null!;
    private ConcurrentInsertInterceptor interceptor = null!;

    private static ConfigurationKey Key(string component = "users-list", string type = "table")
        => new() { ComponentId = component, ConfigurationType = type };

    private static SaveConfigurationRequest Request(string json = "{\"pageSize\":25}", int version = 1)
        => new() { Data = JsonSerializer.Deserialize<JsonElement>(json), SchemaVersion = version };

    [SetUp]
    public async Task SetUp()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        identity = new StubUserContext { User = Principal("alice") };
        interceptor = new ConcurrentInsertInterceptor(connection);
        provider = CreateProvider("app");
        using var scope = provider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<StorageDbContext>().Database.EnsureCreatedAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await provider.DisposeAsync();
        await connection.DisposeAsync();
    }

    [Test]
    public async Task UpsertPersistsOpaqueJsonAndUpdatesExistingRecordAcrossScopes()
    {
        var first = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        var second = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("{\"unknown\":[1,null,{\"nested\":true}]}", 7)));
        var stored = (await Use(storage => storage.GetAsync([Key()]))).Single();

        Assert.Multiple(() =>
        {
            Assert.That(second.Id, Is.EqualTo(first.Id));
            Assert.That(stored.User!.SchemaVersion, Is.EqualTo(7));
            Assert.That(stored.User.Data.GetProperty("unknown")[2].GetProperty("nested").GetBoolean(), Is.True);
            Assert.That(stored.User.UpdatedAt.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(stored.User.UpdatedAt, Is.GreaterThanOrEqualTo(first.UpdatedAt));
            Assert.That(stored.Admin, Is.Null);
        });
        using var scope = provider.CreateScope();
        var rows = await scope.ServiceProvider.GetRequiredService<StorageDbContext>().Set<ComponentConfiguration>().ToListAsync();
        Assert.That(rows, Has.Count.EqualTo(1));
        Assert.That(rows[0].UserId, Is.EqualTo("alice"));
    }

    [Test]
    public async Task UsersCannotReadOverwriteOrDeleteAnotherUsersConfiguration()
    {
        var alice = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        identity.User = Principal("bob");
        Assert.That((await Use(storage => storage.GetAsync([Key()]))).Single().User, Is.Null);
        Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.User)), Is.False);
        var bob = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("[]")));
        Assert.That(bob.Id, Is.Not.EqualTo(alice.Id));
        identity.User = Principal("alice");
        Assert.That((await Use(storage => storage.GetAsync([Key()]))).Single().User!.Id, Is.EqualTo(alice.Id));
    }

    [Test]
    public async Task AdminAndUserLevelsAreReturnedSeparatelyWithoutMerging()
    {
        identity.User = Principal("administrator", true);
        var admin = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.Admin, Request("{\"density\":\"compact\"}")));
        identity.User = Principal("alice");
        var user = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        var result = (await Use(storage => storage.GetAsync([Key()]))).Single();
        Assert.That(result.Admin!.Id, Is.EqualTo(admin.Id));
        Assert.That(result.User!.Id, Is.EqualTo(user.Id));
        Assert.That(result.User.Data.TryGetProperty("density", out _), Is.False);
        Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.User)), Is.True);
        Assert.That((await Use(storage => storage.GetAsync([Key()]))).Single().Admin!.Id, Is.EqualTo(admin.Id));
    }

    [Test]
    public void NormalUsersCannotWriteOrDeleteAdminConfigurations()
    {
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.Admin, Request())));
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.Admin)));
    }

    [Test]
    public async Task AdminCanUpdateAndDeleteSharedConfiguration()
    {
        identity.User = Principal("administrator", true);
        var first = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.Admin, Request()));
        identity.User = Principal("another-admin", true);
        var updated = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.Admin, Request("{}", 2)));
        Assert.That(updated.Id, Is.EqualTo(first.Id));
        Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.Admin)), Is.True);
        Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.Admin)), Is.False);
    }

    [Test]
    public async Task BatchReturnsRequestedPairsAndExplicitMissingLevels()
    {
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        await Use(storage => storage.UpsertAsync(Key("orders", "layout"), ConfigurationLevel.User, Request("[1,2]")));
        var results = await Use(storage => storage.GetAsync([Key(), Key("orders", "layout"), Key("missing"), Key("users-list", "filters")]));
        Assert.That(results, Has.Count.EqualTo(4));
        Assert.That(results[0].User, Is.Not.Null);
        Assert.That(results[1].User!.Data.ValueKind, Is.EqualTo(JsonValueKind.Array));
        Assert.That(results.Skip(2).All(result => result.User is null && result.Admin is null), Is.True);
    }

    [Test]
    public async Task ContextSeparatesConfigurationsIncludingAdminDefaults()
    {
        identity.User = Principal("alice", true);
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.Admin, Request()));
        await using var otherProvider = CreateProvider("other-app");
        using var scope = otherProvider.CreateScope();
        var otherStorage = scope.ServiceProvider.GetRequiredService<IConfigurationStorage>();
        var result = (await otherStorage.GetAsync([Key()])).Single();
        Assert.That(result.Admin, Is.Null);
        Assert.That(result.User, Is.Null);
        Assert.That(await otherStorage.DeleteAsync(Key(), ConfigurationLevel.Admin), Is.False);
        await otherStorage.UpsertAsync(Key(), ConfigurationLevel.User, Request("false"));
        Assert.That((await Use(storage => storage.GetAsync([Key()]))).Single().User!.Data.ValueKind, Is.EqualTo(JsonValueKind.Object));
    }

    [TestCase("{}")]
    [TestCase("[]")]
    [TestCase("null")]
    [TestCase("42")]
    [TestCase("true")]
    [TestCase("\"text\"")]
    public async Task AcceptsAnyJsonValue(string json)
    {
        var response = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request(json)));
        Assert.That(response.Data.GetRawText(), Is.EqualTo(json));
    }

    [Test]
    public void InvalidInputIsRejected()
    {
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(" "), ConfigurationLevel.User, Request())));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(new string('a', 201)), ConfigurationLevel.User, Request())));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request(version: 0))));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, new SaveConfigurationRequest { Data = default })));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(), (ConfigurationLevel)99, Request())));
    }

    [Test]
    public async Task ServiceAcceptsEmptyAndLargeBatches()
    {
        Assert.That(await Use(storage => storage.GetAsync([])), Is.Empty);
        var keys = Enumerable.Range(0, 101).Select(index => Key($"component-{index}")).ToArray();
        Assert.That(await Use(storage => storage.GetAsync(keys)), Has.Count.EqualTo(101));
    }

    [Test]
    public void AnonymousOrMissingStableIdentityIsRejected()
    {
        identity.User = new ClaimsPrincipal(new ClaimsIdentity());
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => Use(storage => storage.GetAsync([Key()])));
        identity.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, "alice")], "test"));
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request())));
    }

    [TestCase(ConfigurationLevel.Admin, null)]
    [TestCase(ConfigurationLevel.User, "alice")]
    public async Task DatabaseEnforcesUniqueness(ConfigurationLevel level, string? owner)
    {
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
        context.Add(Entity(level, owner));
        await context.SaveChangesAsync();
        context.Add(Entity(level, owner));
        Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [TestCase(ConfigurationLevel.Admin, "alice")]
    [TestCase(ConfigurationLevel.User, null)]
    [TestCase((ConfigurationLevel)99, null)]
    public void DatabaseEnforcesLevelOwnership(ConfigurationLevel level, string? owner)
    {
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
        context.Add(Entity(level, owner));
        Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [Test]
    public async Task ConcurrentFirstInsertIsRetriedAsUpdate()
    {
        interceptor.Enabled = true;
        var result = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("{\"winner\":true}", 3)));
        Assert.That(result.Id, Is.EqualTo(interceptor.InsertedId));
        Assert.That(result.SchemaVersion, Is.EqualTo(3));
        Assert.That(result.Data.GetProperty("winner").GetBoolean(), Is.True);
        using var scope = provider.CreateScope();
        Assert.That(await scope.ServiceProvider.GetRequiredService<StorageDbContext>().Set<ComponentConfiguration>().CountAsync(), Is.EqualTo(1));
    }

    private ServiceProvider CreateProvider(string context)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IUserContext>(identity);
        services.AddDbContext<StorageDbContext>(options => options.UseSqlite(connection).AddInterceptors(interceptor));
        services.AddConfigurationStorage<StorageDbContext>(options => options.Context = context);
        return services.BuildServiceProvider();
    }

    private async Task<TResult> Use<TResult>(Func<IConfigurationStorage, Task<TResult>> action)
    {
        using var scope = provider.CreateScope();
        return await action(scope.ServiceProvider.GetRequiredService<IConfigurationStorage>());
    }

    internal static ClaimsPrincipal Principal(string userId, bool admin = false)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId), new(ClaimTypes.Name, userId) };
        if (admin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }

    private static ComponentConfiguration Entity(ConfigurationLevel level, string? owner)
        => new() { Id = Guid.NewGuid(), Context = "app", ComponentId = "users-list", ConfigurationType = "table", Level = level, UserId = owner, Data = "{}" };
}

public sealed class StorageDbContext(DbContextOptions<StorageDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.AddConfigurationStorage();
}

internal sealed class StubUserContext : IUserContext
{
    public IPrincipal User { get; set; } = new ClaimsPrincipal();

    public string GetUserName() => User.Identity?.Name ?? string.Empty;

    public string? GetToken() => null;

    public void SetUser(IPrincipal user) => User = user;

    public void SetToken(string token) { }

    public void ClearInjectedUserAndToken() => User = new ClaimsPrincipal();
}

internal sealed class ConcurrentInsertInterceptor(SqliteConnection connection) : SaveChangesInterceptor
{
    public bool Enabled { get; set; }

    public Guid InsertedId { get; private set; }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (Enabled)
        {
            Enabled = false;
            var pending = eventData.Context!.ChangeTracker.Entries<ComponentConfiguration>().Single(entry => entry.State == EntityState.Added).Entity;
            await using var other = new StorageDbContext(new DbContextOptionsBuilder<StorageDbContext>().UseSqlite(connection).Options);
            InsertedId = Guid.NewGuid();
            other.Add(new ComponentConfiguration
            {
                Id = InsertedId,
                Context = pending.Context,
                ComponentId = pending.ComponentId,
                ConfigurationType = pending.ConfigurationType,
                Level = pending.Level,
                UserId = pending.UserId,
                Data = "{}",
            });
            await other.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}