using System.Text.Json.Serialization;
using Verisoft.Core.AspNet.Concurency.Extensions;
using Verisoft.Core.Authentication.MicrosoftIdentity.Extensions;
using Verisoft.Core.Codebooks.Common.Converters;
using Verisoft.Core.Codebooks.Common.InstallExtensions;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Codebooks.Common.ModelBinders;
using Verisoft.Core.HangFire;
using Verisoft.Core.KeyVault;
using Verisoft.Core.OpenApi.OpenApi;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Data.EF.Context;
using Verisoft.DemoApi.Host.HangfireJobs;
using Verisoft.DemoApi.Host.InstallExtensions;

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
builder.Services.AddCodebooksForConsumer(CodebookDiscovery.DiscoverFromAssembly(typeof(CodebookParentEntity).Assembly));
builder.Services.AddOpenApiSpecification();
builder.Services.AddConcurency<IDemoApiDbContext>();
builder.Services.AddVerisoftIdentity();
builder.Services.AddDemoApi(builder.Configuration);
if (builder.Configuration.GetValue<bool?>("Pdf:DownloadBrowser") == true)
{
    Console.WriteLine("Downloading browser");
    await Verisoft.Core.Pdf.Extensions.InstallExtensions.DownloadBrowser();
    Console.WriteLine("Browser downloaded");
}

var app = builder.Build();
app.Logger.LogInformation("Starting Demo API in {EnvironmentName}", app.Environment.EnvironmentName);

app.UseVerisoftIdentity();
app.UsePathBase("/api");
app.UseDemoApi(builder.Configuration);
app.UseRouting();
app.AddOpenApi(o => { o.Url = "api/"; });
app.UseAuthentication();
app.UseAuthorization();
app.UseVerisoftHangfire();
app.MapHealthChecks("/health");
app.RegisterCronJobs();
app.MapControllers();
app.Run();
