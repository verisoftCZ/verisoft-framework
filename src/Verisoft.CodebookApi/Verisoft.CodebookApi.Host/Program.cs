using System.Text.Json.Serialization;
using Verisoft.CodebookApi.Host.InstallExtensions;
using Verisoft.Core.AspNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCodebookApi(builder.Configuration);

var app = builder.Build();

app.UsePathBase("/api");
app.AddOpenApi();
app.UseCodebookApi();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.ConfigureCodebookRecordControllers();
app.MapControllers();

app.Run();
