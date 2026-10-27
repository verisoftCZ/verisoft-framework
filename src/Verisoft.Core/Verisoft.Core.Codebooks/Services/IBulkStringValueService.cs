using Microsoft.AspNetCore.Http;
using Verisoft.Core.Codebooks.Common.Models.Bulk;

namespace Verisoft.Core.Codebooks.Services;

public interface IBulkStringValueService
{
    Task<BulkImportResult> ImportStringValueDetailFromExcelFileAsync(IFormFile file);
}