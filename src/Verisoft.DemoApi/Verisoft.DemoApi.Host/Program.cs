using System.Text.Json.Serialization;
using Verisoft.Core.AspNet;
using Verisoft.Core.AspNet.Concurency.Extensions;
using Verisoft.Core.Authentication.MicrosoftIdentity.Extensions;
using Verisoft.DemoApi.Data.EF.Context;
using Verisoft.DemoApi.Host.HangfireJobs;
using Verisoft.DemoApi.Host.InstallExtensions;

var builder = WebApplication.CreateBuilder(args);
if (builder.Configuration["AzureAd:ClientSecret"] != null)
{
    builder.AddAzureKeyVault(builder.Configuration);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddOpenApiSpecification();
builder.Services.AddConcurency<IDemoApiDbContext>();
builder.Services.AddDemoApi(builder.Configuration);
builder.Services.AddVerisoftIdentity();
if (builder.Environment.EnvironmentName == "DEVExternal")
{
    await Verisoft.Core.Pdf.Extensions.InstallExtensions.DownloadBrowser();
}

var app = builder.Build();
app.UseVerisoftIdentity();
app.UsePathBase("/api");
app.AddOpenApi();
//app.UseConcurency();
app.UseDemoApi();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.UseHangfire();
app.RegisterCronJobs();
app.MapControllers();
app.Run();
