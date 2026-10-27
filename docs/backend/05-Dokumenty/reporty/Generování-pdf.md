# PDF z HTML template

Generování PDF ve frameworku funguje ve dvou krocích: nejprve se do HTML
šablony naplní data (`Verisoft.Core.Template`), následně se hotové HTML
vyrenderuje do PDF (`Verisoft.Core.Pdf`).

## Template service

`ITemplateService.FillTemplate(string template, object templateData)`
(`Verisoft.Core.Template.Services`) naplní placeholdery v HTML šabloně
hodnotami z předaného objektu. Implementace (`TemplateService`) používá
knihovnu **Mustache** (`FormatCompiler`/`Generator`), syntaxe šablony je tedy
Mustache/Handlebars-like:

```
<div>
    <h1>{{Header}}</h1>
    <p>Datetime: {{Date:dd.MM.yyyy hh:mm:ss}}</p>
    <ul>
        {{#each ListData}}
        <li>{{Firstname}} - {{Surname}}</li>
        {{/each}}
    </ul>
</div>
```

- `{{Property}}` – vloží hodnotu vlastnosti.
- `{{Property:format}}` – vloží hodnotu formátovanou dle standardního .NET
  format stringu (výše `DateTime` formátovaný jako `dd.MM.yyyy hh:mm:ss`).
- `{{#each Collection}} ... {{/each}}` – iterace přes kolekci, uvnitř bloku se
  odkazuje na vlastnosti prvku kolekce.

## PDF service

`IPdfService.GenerateFromHtml(string html)` (`Verisoft.Core.Pdf.Services`)
vrací `Task<Stream>` s výsledným PDF. Implementace (`PdfService`) používá
**PuppeteerSharp** – spustí headless Chromium (`Puppeteer.LaunchAsync`),
nastaví HTML obsah stránky (`SetContentAsync`) a vyrenderuje PDF
(`PdfStreamAsync`).

Protože PuppeteerSharp potřebuje binárku Chromia, je nutné ji před prvním
použitím stáhnout – k tomu slouží `InstallExtensions.DownloadBrowser()`
(`Verisoft.Core.Pdf.Extensions`), typicky volané při startu aplikace nebo
v rámci CI/CD build kroku.

## Registrace

```csharp
private static void RegisterTemplates(IServiceCollection serviceCollection)
{
    serviceCollection.AddTemplateService();
}

private static void RegisterPdfGenerator(IServiceCollection serviceCollection)
{
    serviceCollection.AddPdfService();
}
```

`Templates` slouží pro naplnění dat do HTML šablony, `Pdf generator` pak pro
vygenerování PDF z výsledného HTML.

## Příklad použití (DemoApi)

`DemoPdfService` (`Verisoft.DemoApi.Application.Services`) naplní HTML
šablonu daty a pošle výsledek do PDF generátoru:

```csharp
public class DemoPdfService(IPdfService pdfService, ITemplateService templateService) : IDemoPdfService
{
    private readonly string htmlTemplate = """
                                             <div>
                                                 <h1>{{Header}}</h1>
                                                 <p>Datetime: {{Date:dd.MM.yyyy hh:mm:ss}}</p>
                                                 <ul>
                                                     {{#each ListData}}
                                                     <li>{{Firstname}} - {{Surname}}</li>
                                                     {{/each}}
                                                 </ul>
                                             </div>
                                           """;

    public async Task<BusinessActionResult<Stream>> DownloadPdfAsync(string pdfData)
    {
        MainData data = new()
        {
            Header = pdfData,
            Date = DateTime.UtcNow,
            ListData =
            [
                new MainDataItem { Firstname = "Karel", Surname = "Omáčka" },
                new MainDataItem { Firstname = "Honza", Surname = "Novák" },
                new MainDataItem { Firstname = "Velký", Surname = "Malá" },
                new MainDataItem { Firstname = "abcxyz", Surname = "Dloooooooouhý" }
            ],
        };

        // Fill the template with data
        string filled = templateService.FillTemplate(htmlTemplate, data);
        return await pdfService.GenerateFromHtml(filled);
    }
}
```

`pdfService.GenerateFromHtml(filled)` vrací `Task<Stream>`, který se implicitně
konvertuje na `BusinessActionResult<Stream>` (úspěšný výsledek s daným
streamem) díky implicitnímu operátoru na `BusinessActionResult<T>`.

Příklad controlleru:

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class PdfController(IDemoPdfService demoPdfService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Stream))]
    public async Task<IActionResult?> GeneratePdfAsync(string pdfData)
    {
        var res = await demoPdfService.DownloadPdfAsync(pdfData);
        return res.ToActionResult();
    }
}
```
