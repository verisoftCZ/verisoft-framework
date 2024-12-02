using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using Verisoft.Core.Pdf.Services;

namespace Verisoft.Core.Pdf.Extensions;

public static class InstallExtensions
{
    public static IServiceCollection AddPdfService(this IServiceCollection services)
    {
        services.AddScoped<IPdfService, PdfService>();
        return services;
    }

    public static Task DownloadBrowser()
    {
        var browserFetcher = new BrowserFetcher();
        return browserFetcher.DownloadAsync();
    }
}