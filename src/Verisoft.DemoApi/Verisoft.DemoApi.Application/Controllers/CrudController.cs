using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Services.Interfaces;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation;
using Verisoft.Core.Contracts.Validation.Extensions;
using Verisoft.Core.Validation.Services;

namespace Verisoft.DemoApi.Application.Controllers
{
    public class CrudController<TContract, TCreateModel, TEditModel, TValidatedObject, TListItem, TFilter, TEntity, TKey>(
        ICrudService<TContract, TCreateModel, TEditModel, TListItem, TFilter, TEntity, TKey> service,
        IBaseExportService<TListItem, TFilter, TKey> exportService,
        IValidationService validationService)
        : BaseReadController<TContract, TCreateModel, TEditModel, TValidatedObject, TListItem, TFilter, TEntity, TKey>(service, exportService)
    where TValidatedObject : class, IValidatedObject
    where TEditModel : class
    where TCreateModel : class
    {
        private readonly ICrudService<TContract, TCreateModel, TEditModel, TListItem, TFilter, TEntity, TKey> service = service ?? throw new ArgumentNullException(nameof(service));
        private readonly IValidationService validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

        protected virtual async Task<IActionResult?> AddAsync(TCreateModel editModel)
        {
            var validationResult = await validationService.ValidateAsync<TValidatedObject, TCreateModel>(editModel);

            if (validationResult.HasValidationErrors())
            {
                return validationResult.ToActionResult();
            }

            var result = await service.AddAsync(editModel);
            return result.ToActionResult();
        }

        protected virtual async Task<IActionResult?> UpdateAsync(TKey id, TEditModel editModel)
        {
            var validationResult = await validationService.ValidateAsync<TValidatedObject, TEditModel>(editModel, id);

            if (validationResult.HasValidationErrors())
            {
                return validationResult.ToActionResult();
            }

            var result = await service.UpdateAsync(id, editModel);
            return result.ToActionResult();
        }

        protected virtual async Task<IActionResult?> DeleteAsync(TKey id)
        {
            var result = await service.RemoveAsync(id);
            return result.ToActionResult();
        }

        protected virtual async Task<IActionResult> AddBulkAsync(TCreateModel[] createModels)
        {
            var result = await service.AddBulkAsync(createModels);
            return result.ToActionResult();
        }

        protected virtual async Task<IActionResult> DeleteBulkAsync(List<TKey> ids)
        {
            var result = await service.RemoveBulkAsync(ids);
            return result.ToActionResult();
        }
    }
}
