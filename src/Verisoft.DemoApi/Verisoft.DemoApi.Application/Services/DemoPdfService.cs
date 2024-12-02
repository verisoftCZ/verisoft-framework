using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Pdf.Services;
using Verisoft.Core.Template.Services;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Models.Template;

namespace Verisoft.DemoApi.Application.Services;

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