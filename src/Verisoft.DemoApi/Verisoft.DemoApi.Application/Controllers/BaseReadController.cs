using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Export;
using Verisoft.Core.Contracts.Validation;
using Verisoft.Core.Contracts.Validation.Extensions;
using Verisoft.Core.Export.ExportResult;

namespace Verisoft.DemoApi.Application.Controllers
{
    public abstract class BaseReadController<TContract, TCreateModel, TEditModel, TValidatedObject, TListItem, TFilter, TEntity, TKey>(
        IReadService<TContract, TListItem, TFilter, TEntity, TKey> service,
        IBaseExportService<TListItem, TFilter, TKey> exportService)
        : ControllerBase
        where TValidatedObject : class, IValidatedObject
        where TEditModel : class
    {
        private readonly IReadService<TContract, TListItem, TFilter, TEntity, TKey> service = service ?? throw new ArgumentNullException(nameof(service));
        private readonly IBaseExportService<TListItem, TFilter, TKey> exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));

        protected virtual async Task<bool> DoesExistAsync(TKey id) => await service.ExistsAsync(id);

        protected virtual async Task<PagedData<TListItem>> GetListAsync(FilteredRequest<TFilter> request) => await service.GetListAsync(request);

        protected virtual async Task<IActionResult?> GetAsync(TKey id)
        {
            var result = await service.GetAsync(id);
            return result.ToActionResult();
        }

        protected virtual async Task<FileStreamResult> ExportDataAsync(ExportDataRequest<TFilter> request)
        {
            ExportFileResult? exportResult = null;
            try
            {
                exportResult = await exportService.ExportDataAsync(request);
                return File(exportResult.FileStream!, exportResult.FileType!, exportResult.FileName);
            }
            catch (Exception)
            {
                if (exportResult is not null)
                {
                    await exportResult.FileStream!.DisposeAsync();
                }

                throw;
            }
        }
    }
}