using System.Text.Json.Serialization;
using Verisoft.CodebookApi.Host.Context;
using Verisoft.CodebookApi.Host.InstallExtensions;
using Verisoft.Core.AspNet;
using Verisoft.Core.Codebooks.Common.Converters;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Common.ModelBinders;
using Verisoft.Core.Codebooks.InstallExtensions;
using Verisoft.Core.KeyVault;
using Verisoft.Core.OpenApi.OpenApi;
using Verisoft.DemoApi.Common.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureKeyVault(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new CodebookRecordModelBinderProvider());
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new CodebookRecordJsonConverterFactory());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddCodebooks(builder.Configuration, CodebookDiscovery.DiscoverFromAssembly(typeof(CodebookParentEntity).Assembly));
builder.Services.AddDemoCodebookApi(builder.Configuration);

var app = builder.Build();
app.Logger.LogInformation("Starting Codebook API in {EnvironmentName}", app.Environment.EnvironmentName);

app.UsePathBase("/api/codebook");
app.AddOpenApi(x =>
{
    x.Url = "api/codebook";
});

app.UseExceptionHandler();
app.MigrateDatabase<IDemoApiCodebookDbContext>();
await app.UseCodebookApi();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
