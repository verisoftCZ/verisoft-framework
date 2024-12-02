Registrace:
    
``` csharp
private static void RegisterTemplates(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTemplateService();
    }

    private static void RegisterPdfGenerator(IServiceCollection serviceCollection)
    {
        serviceCollection.AddPdfService();
    }
```
Templates jsou pro naplnění dat do HTML templaty.
Pdf generator je pak pro generování PDF za HTML templaty.


Příklad PDF controlleru:
``` csharp
public class PdfController : ControllerBase
{
    private readonly IDemoPdfService demoPdfService;

    public PdfController(IDemoPdfService demoPdfService)
    {
        this.demoPdfService = demoPdfService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Stream))]
    public async Task<IActionResult> GeneratePdfAsync(string pdfData)
    {
        var res = await demoPdfService.DownloadPdfAsync(pdfData);
        return res.ToActionResult();
    }
}
```

DemoPdfService - naplní html template daty, a pošle do pdf generátoru, který z html udělá pdf:

``` csharp
public class DemoPdfService : IDemoPdfService
{
    private readonly IPdfService pdfService;
    private readonly ITemplateService templateService;

    private string htmlTemplate = """
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

    public DemoPdfService(IPdfService pdfService, ITemplateService templateService)
    {
        this.pdfService = pdfService;
        this.templateService = templateService;
    }

    public async Task<BusinessActionResult<Stream>> DownloadPdfAsync(string pdfData)
    {
        MainData data = new() { Header = pdfData };
        data.Date = DateTime.UtcNow;
        data.ListData =
        [
            new MainDataItem { Firstname = "Karel", Surname = "Omáčka" },
            new MainDataItem { Firstname = "Honza", Surname = "Novák" },
            new MainDataItem { Firstname = "Velký", Surname = "Malá" },
            new MainDataItem { Firstname = "abcxyz", Surname = "Dloooooooouhý" }
        ];

        // Fill the template with data
        string filled = templateService.FillTemplate(htmlTemplate, data);
        return await pdfService.GenerateFromHtml(filled);
    }
}
```

