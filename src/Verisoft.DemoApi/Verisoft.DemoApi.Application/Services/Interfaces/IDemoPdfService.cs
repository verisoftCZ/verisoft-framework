using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface IDemoPdfService
{
    public Task<BusinessActionResult<Stream>> DownloadPdfAsync(string pdfData);
}