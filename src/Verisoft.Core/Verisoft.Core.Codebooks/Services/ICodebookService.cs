using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

public interface ICodebookService
{
    Task<BusinessActionResult<Codebook>> AddCodebookAsync(CodebookEditModel codebookEditModel);

    Task<BusinessActionResult<Codebook>> GetCodebookAsync(int id);

    Task<BusinessActionResult<CodebookListData>> GetCodebooksAsync(FilteredRequest<CodebookFilter> request);

    Task<BusinessActionResult<Codebook>> RemoveCodebookAsync(int id);

    Task<BusinessActionResult<Codebook>> UpdateCodebookAsync(int id, CodebookEditModel codebookEditModel);
}