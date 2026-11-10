using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Verisoft.Core.AspNet;
using Verisoft.Core.AspNet.Concurency.Extensions;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.ConfigurationStorage.Test;

[TestFixture]
public class ConfigurationStorageApiTests
{
    private SqliteConnection connection = null!;
    private IHost host = null!;
    private TestServer server = null!;
    private HttpClient client = null!;
    private const string Root = "/configurations";
    private const string Single = Root + "/users-list/table";

    [SetUp]
    public async Task SetUp()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        connection.CreateCollation("Latin1_General_100_BIN2", string.CompareOrdinal);
        host = await new HostBuilder().ConfigureWebHost(webBuilder => webBuilder.UseTestServer()
            .ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddProblemDetails();
                services.AddConcurency<StorageDbContext>();
                services.AddVerisoftExceptionHandlers();
                services.AddHttpContextAccessor();
                services.AddScoped<IUserContext>(provider => new StubUserContext
                {
                    User = provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User ?? new System.Security.Claims.ClaimsPrincipal(),
                });
                services.AddAuthentication("test").AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("test", _ => { });
                services.AddDbContext<StorageDbContext>(options => options.UseSqlite(connection));
                services.AddConfigurationStorage<StorageDbContext>();
                services.AddControllers().AddApplicationPart(typeof(TestConfigurationsController).Assembly);
            })
            .Configure(app =>
            {
                app.UseExceptionHandler();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => endpoints.MapControllers());
            })).StartAsync();
        server = host.GetTestServer();
        using var scope = server.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<StorageDbContext>().Database.EnsureCreatedAsync();
        client = server.CreateClient();
        AsUser("alice");
    }

    [TearDown]
    public async Task TearDown()
    {
        client.Dispose();
        host.Dispose();
        await connection.DisposeAsync();
    }

    [Test]
    public async Task GlobalValidationWorksOutsideConfigurationController()
    {
        var response = await client.GetAsync("/shared-errors/validation");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.That(problem!.Errors["name"], Is.EqualTo(new[] { "Name is required." }));
    }

    [Test]
    public async Task ConcurrencyErrorsUseFramework409Handler()
    {
        var response = await client.GetAsync("/shared-errors/concurrency");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.That(problem!.Title, Is.EqualTo("Concurrency conflict"));
    }

    [TestCase("forbidden", HttpStatusCode.Forbidden)]
    [TestCase("unexpected", HttpStatusCode.InternalServerError)]
    public async Task GlobalErrorsDoNotExposeExceptionDetails(string kind, HttpStatusCode expected)
    {
        var response = await client.GetAsync("/shared-errors/" + kind);
        Assert.That(response.StatusCode, Is.EqualTo(expected));
        Assert.That(await response.Content.ReadAsStringAsync(), Does.Not.Contain("private exception detail"));
    }

    [Test]
    public async Task ServiceValidationUsesGlobalProblemDetails()
    {
        var response = await client.GetAsync(Root + "/%20users-list/table");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.That(problem!.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task InvalidOwnerUsesGlobalForbiddenResponse()
    {
        AsUser(new string('a', 201));
        var response = await client.GetAsync(Single);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.That(problem!.Title, Is.EqualTo("Forbidden"));
        Assert.That(problem.Detail, Is.Null);
    }

    [Test]
    public async Task AnonymousRequestsReturn401()
    {
        client.DefaultRequestHeaders.Clear();
        Assert.That((await client.GetAsync(Single)).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        Assert.That((await Put("user")).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        Assert.That((await client.DeleteAsync(Single + "/user")).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task RegularUserCannotWriteOrDeleteAdminLevel()
    {
        Assert.That((await Put("admin")).StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        Assert.That((await client.DeleteAsync(Single + "/admin")).StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task CompleteWorkflowPreservesJsonAndUserIsolation()
    {
        AsUser("administrator", true);
        Assert.That((await Put("admin", "{\"data\":{\"density\":\"compact\"},\"schemaVersion\":1}")).StatusCode, Is.EqualTo(HttpStatusCode.OK));
        AsUser("alice");
        var saved = await Put("user");
        Assert.That(saved.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var original = await saved.Content.ReadFromJsonAsync<ConfigurationResponse>();
        var updated = await (await Put("user", "{\"data\":[1,null,true],\"schemaVersion\":3}")).Content.ReadFromJsonAsync<ConfigurationResponse>();
        Assert.That(updated!.Id, Is.EqualTo(original!.Id));
        var available = await client.GetFromJsonAsync<ComponentConfigurationsResponse>(Single);
        Assert.That(available!.Admin!.Data.GetProperty("density").GetString(), Is.EqualTo("compact"));
        Assert.That(available.User!.Data.ValueKind, Is.EqualTo(JsonValueKind.Array));

        AsUser("bob");
        var bob = await client.GetFromJsonAsync<ComponentConfigurationsResponse>(Single);
        Assert.That(bob!.Admin, Is.Not.Null);
        Assert.That(bob.User, Is.Null);
        Assert.That((await client.DeleteAsync(Single + "/user")).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        AsUser("alice");
        Assert.That((await client.DeleteAsync(Single + "/user")).StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        available = await client.GetFromJsonAsync<ComponentConfigurationsResponse>(Single);
        Assert.That(available!.User, Is.Null);
        Assert.That(available.Admin, Is.Not.Null);
        AsUser("administrator", true);
        Assert.That((await client.DeleteAsync(Single + "/admin")).StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task BatchAndMissingConfigurationsHaveUnambiguousResults()
    {
        var missing = await client.GetFromJsonAsync<ComponentConfigurationsResponse>(Single);
        Assert.That(missing!.Admin, Is.Null);
        Assert.That(missing.User, Is.Null);
        await Put("user");
        var response = await client.PostAsJsonAsync(Root + "/query", new
        {
            configurations = new[]
            {
                new { componentId = "users-list", configurationType = "table" },
                new { componentId = "dashboard", configurationType = "widgets" },
            },
        });
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var results = await response.Content.ReadFromJsonAsync<ComponentConfigurationsResponse[]>();
        Assert.That(results, Has.Length.EqualTo(2));
        Assert.That(results![0].User, Is.Not.Null);
        Assert.That(results[1].User, Is.Null);
        Assert.That(results[1].Admin, Is.Null);
    }

    [TestCase("{}")]
    [TestCase("{\"schemaVersion\":1}")]
    [TestCase("{\"data\":{},\"schemaVersion\":0}")]
    [TestCase("{\"data\":not-json}")]
    public async Task InvalidPayloadReturns400(string payload)
        => Assert.That((await Put("user", payload)).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

    [TestCase(0)]
    [TestCase(101)]
    public async Task BatchSizeIsNotRestricted(int count)
    {
        var response = await client.PostAsJsonAsync(Root + "/query", new
        {
            configurations = Enumerable.Range(0, count)
                .Select(index => new { componentId = $"component-{index}", configurationType = "table" })
                .ToArray(),
        });
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var results = await response.Content.ReadFromJsonAsync<ComponentConfigurationsResponse[]>();
        Assert.That(results, Has.Length.EqualTo(count));
    }

    [TestCase("{}")]
    [TestCase("{\"configurations\":null}")]
    [TestCase("{\"configurations\":[null]}")]
    [TestCase("{\"configurations\":[{\"componentId\":\"\",\"configurationType\":\"table\"}]}")]
    public async Task InvalidBatchReturns400(string payload)
    {
        var response = await client.PostAsync(Root + "/query", new StringContent(payload, Encoding.UTF8, "application/json"));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task SuppliedUserIdCannotImpersonateAnotherUser()
    {
        await Put("user", "{\"data\":{},\"userId\":\"bob\",\"context\":\"other\"}");
        Assert.That((await client.GetFromJsonAsync<ComponentConfigurationsResponse>(Single))!.User, Is.Not.Null);
        AsUser("bob");
        Assert.That((await client.GetFromJsonAsync<ComponentConfigurationsResponse>(Single))!.User, Is.Null);
    }

    private Task<HttpResponseMessage> Put(string level, string payload = "{\"data\":{\"pageSize\":25}}")
        => client.PutAsync(Single + "/" + level, new StringContent(payload, Encoding.UTF8, "application/json"));

    private void AsUser(string user, bool admin = false)
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("X-Test-User", user);
        if (admin)
        {
            client.DefaultRequestHeaders.Add("X-Test-Admin", "true");
        }
    }
}

[Route("configurations")]
public sealed class TestConfigurationsController(IConfigurationStorage storage) : ConfigurationStorageControllerBase(storage);

[ApiController]
[Route("shared-errors")]
public sealed class TestErrorsController : ControllerBase
{
    [HttpGet("{kind}")]
    public IActionResult Get(string kind) => throw (kind switch
    {
        "concurrency" => new DbUpdateConcurrencyException("concurrent update"),
        "validation" => new System.ComponentModel.DataAnnotations.ValidationException(
            new System.ComponentModel.DataAnnotations.ValidationResult("Name is required.", ["name", "name"]), null, null),
        "forbidden" => new UnauthorizedAccessException("private exception detail"),
        _ => new InvalidOperationException("private exception detail"),
    });
}

internal sealed class TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var user = Request.Headers["X-Test-User"].ToString();
        return Task.FromResult(string.IsNullOrEmpty(user)
            ? AuthenticateResult.NoResult()
            : AuthenticateResult.Success(new AuthenticationTicket(
                ConfigurationStorageTests.Principal(user, Request.Headers["X-Test-Admin"] == "true"), Scheme.Name)));
    }
}