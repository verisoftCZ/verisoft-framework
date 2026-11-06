using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework;
using Verisoft.Core.Data.EntityFramework.Walkers;

namespace Verisoft.Core.ConfigurationStorage.Test;

[TestFixture]
public class ConfigurationStorageTests
{
    private SqliteConnection connection = null!;
    private ServiceProvider provider = null!;
    private StubUserContext identity = null!;
    private ConcurrentInsertInterceptor interceptor = null!;
    private QueryCommandInterceptor queryInterceptor = null!;

    private static ConfigurationKey Key(string component = "users-list", string type = "table")
        => new() { ComponentId = component, ConfigurationType = type };

    private static SaveConfigurationRequest Request(string json = "{\"pageSize\":25}", int version = 1)
        => new() { Data = JsonSerializer.Deserialize<JsonElement>(json), SchemaVersion = version };

    [SetUp]
    public async Task SetUp()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        connection.CreateCollation("Latin1_General_100_BIN2", string.CompareOrdinal);
        identity = new StubUserContext { User = Principal("alice") };
        interceptor = new ConcurrentInsertInterceptor(connection);
        queryInterceptor = new QueryCommandInterceptor();
        provider = CreateProvider("app");
        using var scope = provider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<StorageDbContext>().Database.EnsureCreatedAsync();
        queryInterceptor.Clear();
    }

    [TearDown]
    public async Task TearDown()
    {
        await provider.DisposeAsync();
        await connection.DisposeAsync();
    }

    [Test]
    public async Task FrameworkAuditDoesNotReplaceStableOwnership()
    {
        identity.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "alice"), new Claim(ClaimTypes.Name, "Alice Display")], "test"));
        var first = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
            var repository = scope.ServiceProvider.GetRequiredService<IComponentConfigurationRepository>();
            Assert.That(repository.UnitOfWork, Is.SameAs(context));
            var entity = await context.Set<ComponentConfiguration>().SingleAsync();
            Assert.That(entity.CreatedBy, Is.EqualTo("Alice Display"));
            Assert.That(entity.CreatedAt, Is.Not.Null);
            Assert.That(entity.UserId, Is.EqualTo("alice"));
            Assert.That(entity.UpdatedAt, Is.Null);
            Assert.That(first.UpdatedAt.UtcDateTime, Is.EqualTo(entity.CreatedAt));
        }

        identity.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "alice"), new Claim(ClaimTypes.Name, "Alice Renamed")], "test"));
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("[]")));
        using var updatedScope = provider.CreateScope();
        var updated = await updatedScope.ServiceProvider.GetRequiredService<StorageDbContext>().Set<ComponentConfiguration>().SingleAsync();
        Assert.That(updated.CreatedBy, Is.EqualTo("Alice Display"));
        Assert.That(updated.UpdatedBy, Is.EqualTo("Alice Renamed"));
        Assert.That(updated.UserId, Is.EqualTo("alice"));
        Assert.That(updated.UpdatedAt, Is.Not.Null);
    }

    [TestCase(ConfigurationLevel.User)]
    [TestCase(ConfigurationLevel.Admin)]
    public async Task SoftDeletePreservesAuditAndAllowsRecreation(ConfigurationLevel level)
    {
        identity.User = Principal("administrator", true);
        var first = await Use(storage => storage.UpsertAsync(Key(), level, Request()));
        Assert.That(await Use(storage => storage.DeleteAsync(Key(), level)), Is.True);
        Assert.That(await Use(storage => storage.DeleteAsync(Key(), level)), Is.False);
        var missing = (await Use(storage => storage.GetAsync([Key()]))).Single();
        Assert.That(missing.Admin, Is.Null);
        Assert.That(missing.User, Is.Null);
        var recreated = await Use(storage => storage.UpsertAsync(Key(), level, Request("[]")));
        Assert.That(recreated.Id, Is.Not.EqualTo(first.Id));
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
        Assert.That(await context.Set<ComponentConfiguration>().CountAsync(), Is.EqualTo(1));
        var removed = await context.Set<ComponentConfiguration>().IgnoreQueryFilters().SingleAsync(entity => entity.Id == first.Id);
        Assert.That(removed.IsDeleted, Is.True);
        Assert.That(removed.UpdatedBy, Is.EqualTo("administrator"));
        Assert.That(removed.UpdatedAt, Is.Not.Null);
        Assert.That(await context.Set<ComponentConfiguration>().IgnoreQueryFilters().CountAsync(), Is.EqualTo(2));
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
    public async Task UserIdsAndConfigurationKeysUseOrdinalCaseSensitiveEquality()
    {
        var lowerCase = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("{\"owner\":\"lower\"}")));
        identity.User = Principal("Alice");
        Assert.That((await Use(storage => storage.GetAsync([Key()]))).Single().User, Is.Null);
        var upperCase = await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("{\"owner\":\"upper\"}")));
        Assert.That(upperCase.Id, Is.Not.EqualTo(lowerCase.Id));

        identity.User = Principal("alice");
        var differentlyCasedKey = Key("Users-list");
        Assert.That((await Use(storage => storage.GetAsync([differentlyCasedKey]))).Single().User, Is.Null);
        var differentlyCasedRecord = await Use(storage => storage.UpsertAsync(differentlyCasedKey, ConfigurationLevel.User, Request("{\"key\":\"upper\"}")));
        Assert.That(differentlyCasedRecord.Id, Is.Not.EqualTo(lowerCase.Id));
        Assert.That((await Use(storage => storage.GetAsync([Key(), differentlyCasedKey]))).Select(result => result.User!.Id),
            Is.EqualTo(new[] { lowerCase.Id, differentlyCasedRecord.Id }));
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
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key("users-list "), ConfigurationLevel.User, Request())));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key("users-list", "table "), ConfigurationLevel.User, Request())));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(new string('a', 201)), ConfigurationLevel.User, Request())));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request(version: 0))));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, new SaveConfigurationRequest { Data = default })));
        Assert.ThrowsAsync<ValidationException>(() => Use(storage => storage.UpsertAsync(Key(), (ConfigurationLevel)99, Request())));
    }

    [Test]
    public async Task ServiceAcceptsEmptyAndLargeBatches()
    {
        Assert.That(await Use(storage => storage.GetAsync([])), Is.Empty);
        var keys = Enumerable.Range(0, 601).Select(index => Key($"component-{index}")).ToArray();
        Assert.That(await Use(storage => storage.GetAsync(keys)), Has.Count.EqualTo(601));
    }

    [Test]
    public async Task BatchKeysAreSentAsSqlParameters()
    {
        const string componentId = "component-value-not-in-sql-text";
        await Use(storage => storage.GetAsync([Key(componentId)]));
        Assert.That(queryInterceptor.Commands, Has.Count.EqualTo(1));
        Assert.That(queryInterceptor.Commands[0], Does.Not.Contain(componentId));

        var firstCommand = queryInterceptor.Commands[0];
        queryInterceptor.Clear();
        await Use(storage => storage.GetAsync([Key("another-component-value")]));
        Assert.That(queryInterceptor.Commands, Has.Count.EqualTo(1));
        Assert.That(queryInterceptor.Commands[0], Is.EqualTo(firstCommand));
    }

    [Test]
    public void PrincipalUserIdDefaultsToNameIdentifier()
    {
        IPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "stable-alice"),
            new Claim(ClaimTypes.Name, "Alice Display"),
            new Claim("sub", "external-alice")], "test"));

        Assert.That(principal.GetUserId(), Is.EqualTo("stable-alice"));
        Assert.That(principal.GetUserId("sub"), Is.EqualTo("external-alice"));
        Assert.That(principal.GetUserName(), Is.EqualTo("stable-alice"));
    }

    [Test]
    public void PrincipalUserIdDoesNotFallBackToNameOrAnotherClaim()
    {
        IPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, "alice")], "test"));
        Assert.That(principal.GetUserName(), Is.EqualTo("alice"));
        Assert.Throws<UnauthorizedAccessException>(() => principal.GetUserId());
        Assert.Throws<UnauthorizedAccessException>(() => Principal("alice").GetUserId("sub"));
    }

    [TestCase("")]
    [TestCase(" ")]
    public void PrincipalUserIdRejectsBlankValues(string value)
        => Assert.Throws<UnauthorizedAccessException>(() => Principal(value).GetUserId());

    [TestCase("")]
    [TestCase(" ")]
    public void PrincipalUserIdRejectsBlankClaimTypes(string claimType)
        => Assert.Throws<ArgumentException>(() => Principal("alice").GetUserId(claimType));

    [Test]
    public void PrincipalUserIdRequiresAuthenticatedClaimsPrincipal()
    {
        IPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "alice")]));
        Assert.Throws<UnauthorizedAccessException>(() => anonymous.GetUserId());
        Assert.Throws<UnauthorizedAccessException>(() => new GenericPrincipal(new GenericIdentity("alice"), []).GetUserId());
        Assert.Throws<UnauthorizedAccessException>(() => ((IPrincipal)null!).GetUserId());
    }

    [Test]
    public async Task ConfiguredUserIdClaimControlsStorageOwnership()
    {
        identity.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "alice"),
            new Claim("sub", "external-alice")], "test"));
        await using var customProvider = CreateProvider("app", "sub");
        using var scope = customProvider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<IConfigurationStorage>();
        var saved = await storage.UpsertAsync(Key(), ConfigurationLevel.User, Request());
        Assert.That((await storage.GetAsync([Key()])).Single().User!.Id, Is.EqualTo(saved.Id));
        var entity = await scope.ServiceProvider.GetRequiredService<StorageDbContext>().Set<ComponentConfiguration>().SingleAsync();
        Assert.That(entity.UserId, Is.EqualTo("external-alice"));
        Assert.That((await Use(defaultStorage => defaultStorage.GetAsync([Key()]))).Single().User, Is.Null);
        identity.User = Principal("alice");
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => storage.GetAsync([Key()]));
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

    [Test]
    public void ConfigurationKeysAndOwnerUseBinaryCaseSensitiveCollation()
    {
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
        var entityType = context.GetService<IDesignTimeModel>().Model.FindEntityType(typeof(ComponentConfiguration))!;
        Assert.Multiple(() =>
        {
            Assert.That(entityType.FindProperty(nameof(ComponentConfiguration.Context))!.GetCollation(), Is.EqualTo("Latin1_General_100_BIN2"));
            Assert.That(entityType.FindProperty(nameof(ComponentConfiguration.ComponentId))!.GetCollation(), Is.EqualTo("Latin1_General_100_BIN2"));
            Assert.That(entityType.FindProperty(nameof(ComponentConfiguration.ConfigurationType))!.GetCollation(), Is.EqualTo("Latin1_General_100_BIN2"));
            Assert.That(entityType.FindProperty(nameof(ComponentConfiguration.UserId))!.GetCollation(), Is.EqualTo("Latin1_General_100_BIN2"));
        });
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

    [TestCase(ConfigurationLevel.User, false)]
    [TestCase(ConfigurationLevel.User, true)]
    [TestCase(ConfigurationLevel.Admin, false)]
    [TestCase(ConfigurationLevel.Admin, true)]
    public async Task UpsertRetriesWhenLoadedConfigurationIsDeleted(ConfigurationLevel level, bool recreate)
    {
        identity.User = Principal("administrator", true);
        var original = await Use(storage => storage.UpsertAsync(Key(), level, Request("\"original\"")));
        Guid? recreatedId = null;
        interceptor.BeforeSave = async () =>
        {
            Assert.That(await Use(storage => storage.DeleteAsync(Key(), level)), Is.True);
            if (recreate)
            {
                recreatedId = (await Use(storage => storage.UpsertAsync(Key(), level, Request("\"recreated\"")))).Id;
            }
        };

        var saved = await Use(storage => storage.UpsertAsync(Key(), level, Request("\"latest\"", 3)));
        var available = (await Use(storage => storage.GetAsync([Key()]))).Single();
        var persisted = level == ConfigurationLevel.User ? available.User : available.Admin;
        Assert.That(persisted, Is.Not.Null);
        Assert.That(persisted!.Id, Is.EqualTo(saved.Id));
        Assert.That(persisted.Data.GetString(), Is.EqualTo("latest"));
        Assert.That(persisted.SchemaVersion, Is.EqualTo(3));
        Assert.That(saved.Id, Is.Not.EqualTo(original.Id));
        if (recreate)
        {
            Assert.That(saved.Id, Is.EqualTo(recreatedId));
        }

        using var scope = provider.CreateScope();
        var deleted = await scope.ServiceProvider.GetRequiredService<StorageDbContext>()
            .Set<ComponentConfiguration>().IgnoreQueryFilters().SingleAsync(entity => entity.Id == original.Id);
        Assert.That(deleted.IsDeleted, Is.True);
        Assert.That(deleted.Data, Is.EqualTo("\"original\""));
    }

    [Test]
    public async Task UpsertStopsAfterRepeatedConcurrentDeletes()
    {
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        var attempts = 0;
        async Task DeleteAndRecreate()
        {
            attempts++;
            Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.User)), Is.True);
            await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
            interceptor.BeforeSave = DeleteAndRecreate;
        }

        interceptor.BeforeSave = DeleteAndRecreate;
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("[]"))));
        Assert.That(attempts, Is.EqualTo(3));
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task ConcurrentDeleteDoesNotDeleteRecreatedConfiguration(bool recreate)
    {
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request()));
        interceptor.BeforeSave = async () =>
        {
            Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.User)), Is.True);
            if (recreate)
            {
                await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("[]")));
            }
        };

        Assert.That(await Use(storage => storage.DeleteAsync(Key(), ConfigurationLevel.User)), Is.False);
        var persisted = (await Use(storage => storage.GetAsync([Key()]))).Single().User;
        if (recreate)
        {
            Assert.That(persisted!.Data.ValueKind, Is.EqualTo(JsonValueKind.Array));
        }
        else
        {
            Assert.That(persisted, Is.Null);
        }
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

    [Test]
    public async Task UpsertReplacesFieldsEvenWhenNewValuesMatchTheTrackedSnapshot()
    {
        var original = Request("{\"value\":\"original\"}", 1);
        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, original));

        using var staleScope = provider.CreateScope();
        var staleStorage = staleScope.ServiceProvider.GetRequiredService<IConfigurationStorage>();
        await staleStorage.UpsertAsync(Key(), ConfigurationLevel.User, original);

        await Use(storage => storage.UpsertAsync(Key(), ConfigurationLevel.User, Request("{\"value\":\"newer\"}", 2)));
        await staleStorage.UpsertAsync(Key(), ConfigurationLevel.User, original);

        var persisted = (await Use(storage => storage.GetAsync([Key()]))).Single().User!;
        Assert.That(persisted.Data.GetProperty("value").GetString(), Is.EqualTo("original"));
        Assert.That(persisted.SchemaVersion, Is.EqualTo(1));
    }

    [TestCase(null, "sub")]
    [TestCase("", "sub")]
    [TestCase(" ", "sub")]
    [TestCase(" app", "sub")]
    [TestCase("app ", "sub")]
    [TestCase("app", null)]
    [TestCase("app", "")]
    [TestCase("app", " ")]
    public void OptionsRejectInvalidValues(string? context, string? claimType)
    {
        using var optionsProvider = CreateProvider(context!, claimType!);
        Assert.Throws<OptionsValidationException>(() => _ = optionsProvider.GetRequiredService<IOptions<ConfigurationStorageOptions>>().Value);
    }

    [TestCase(100, true)]
    [TestCase(101, false)]
    public void OptionsValidateContextLength(int length, bool valid)
    {
        using var optionsProvider = CreateProvider(new string('a', length));
        if (valid)
        {
            Assert.That(optionsProvider.GetRequiredService<IOptions<ConfigurationStorageOptions>>().Value.Context, Has.Length.EqualTo(length));
        }
        else
        {
            Assert.Throws<OptionsValidationException>(() => _ = optionsProvider.GetRequiredService<IOptions<ConfigurationStorageOptions>>().Value);
        }
    }

    [Test]
    public void InvalidOptionsFailAtHostStartup()
    {
        using var host = new HostBuilder().ConfigureServices(services =>
            services.AddConfigurationStorage<StorageDbContext>(options => options.Context = null!)).Build();
        Assert.ThrowsAsync<OptionsValidationException>(() => host.StartAsync());
    }

    private ServiceProvider CreateProvider(string context, string userIdClaimType = ClaimTypes.NameIdentifier)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IUserContext>(identity);
        services.AddDbContext<StorageDbContext>(options => options.UseSqlite(connection).AddInterceptors(interceptor, queryInterceptor));
        services.AddConfigurationStorage<StorageDbContext>(options =>
        {
            options.Context = context;
            options.UserIdClaimType = userIdClaimType;
        });
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

public sealed class StorageDbContext(DbContextOptions<StorageDbContext> options, IUserContext userContext)
    : EntityFrameworkDbContextBase(userContext, new WalkerFactory(NullLogger<CascadeWalker>.Instance), TimeProvider.System)
{
    public DbSet<ComponentConfiguration> ComponentConfigurations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        foreach (var extension in options.Extensions)
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        }

        optionsBuilder.AddInterceptors(
            new Verisoft.Core.AspNet.Concurency.DataInterceptors.OptimisticConcurrencyInterceptor(),
            new SqliteRowVersionInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddConfigurationStorage();
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ComponentConfiguration>().Property(configuration => configuration.Version).ValueGeneratedNever();
    }
}

internal sealed class SqliteRowVersionInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        foreach (var entry in eventData.Context!.ChangeTracker.Entries<ComponentConfiguration>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            entry.Property(configuration => configuration.Version).CurrentValue = System.Security.Cryptography.RandomNumberGenerator.GetBytes(8);
        }

        return ValueTask.FromResult(result);
    }
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

    public Func<Task>? BeforeSave { get; set; }

    public Guid InsertedId { get; private set; }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (Enabled)
        {
            Enabled = false;
            var pending = eventData.Context!.ChangeTracker.Entries<ComponentConfiguration>().Single(entry => entry.State == EntityState.Added).Entity;
            await using var other = new StorageDbContext(new DbContextOptionsBuilder<StorageDbContext>().UseSqlite(connection).Options,
                new StubUserContext { User = ConfigurationStorageTests.Principal("concurrent-writer") });
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

        var beforeSave = BeforeSave;
        BeforeSave = null;
        if (beforeSave is not null)
        {
            await beforeSave();
        }

        return result;
    }
}

internal sealed class QueryCommandInterceptor : DbCommandInterceptor
{
    public List<string> Commands { get; } = [];

    public void Clear() => Commands.Clear();

    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        Commands.Add(command.CommandText);
        return ValueTask.FromResult(result);
    }
}