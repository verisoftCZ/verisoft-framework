using System.Collections.Generic;
using System.Threading.Tasks;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.AspNet.Services.Interfaces;

public interface ICrudService<TContract, TCreateModel, TEditModel, TListItem, TFilter, TEntity, TKey>
    : IReadService<TContract, TListItem, TFilter, TEntity, TKey>
{
    Task<BusinessActionResult<TContract>> AddAsync(TCreateModel createModel);

    Task<BusinessActionResult<TContract>> UpdateAsync(TKey id, TEditModel editModel);

    Task<BusinessActionResult> RemoveAsync(TKey id);

    Task<BusinessActionResult> AddBulkAsync(TCreateModel[] createModels);

    Task<BusinessActionResult> RemoveBulkAsync(List<TKey> ids);
}
