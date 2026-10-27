using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.CodebookParent;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface ICodebookParentService
{
    Task<PagedData<CodebookParentResponse>> GetPagedAsync(FilteredRequest<CodebookParentFilter> request);

    Task<BusinessActionResult<CodebookParentResponse>> GetByIdAsync(int id);

    Task<BusinessActionResult<CodebookParentResponse>> CreateAsync(CodebookParentEditModel model);

    Task<BusinessActionResult<bool>> ExistsForClientAsync(int clientId);
}