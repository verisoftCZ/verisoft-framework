using PuppeteerSharp;

namespace Verisoft.Core.Pdf.Services;

public class PdfService : IPdfService
{
    public async Task<Stream> GenerateFromHtml(string html)
    {
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = ["--no-sandbox", "--disable-dev-shm-usage"]
        });

        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        return await page.PdfStreamAsync();
    }
}