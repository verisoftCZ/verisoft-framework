using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Codebook;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services.Interfaces;

public interface ICodebookService
{
    Task<BusinessActionResult<Codebook>> AddCodebookAsync(CodebookEditModel codebookEditModel);

    Task<BusinessActionResult<Codebook>> GetCodebookAsync(int id);

    Task<BusinessActionResult<CodebookListData>> GetCodebooksAsync(Verisoft.Core.Contracts.FilteredRequest<CodebookFilter> request);

    Task<BusinessActionResult<Codebook>> RemoveCodebookAsync(int id);

    Task<BusinessActionResult<Codebook>> UpdateCodebookAsync(int id, CodebookEditModel codebookEditModel);
}