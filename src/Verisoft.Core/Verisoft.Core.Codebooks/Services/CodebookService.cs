using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Common.Specifications;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Codebooks.Sorts;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

public class CodebookService(ICodebookRepository codebookRepository, ITypeMapper typeMapper) : ICodebookService
{
    private readonly ICodebookRepository codebookRepository = codebookRepository ?? throw new ArgumentNullException(nameof(codebookRepository));
    private readonly ITypeMapper typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));

    public async Task<BusinessActionResult<CodebookListData>> GetCodebooksAsync(FilteredRequest<CodebookFilter> request)
    {
        request.Filter ??= new();
        var specification = typeMapper.Map<CodebookSpecification>(request.Filter);
        var filter = specification.SatisfiedBy();
        var (entities, count) = await codebookRepository.GetPagedSortAsync<CodebookSort>(request.Offset, request.Limit, filter, request.ParsedSorts);

        return new CodebookListData
        {
            Data = typeMapper.Map<Codebook>(entities),
            Total = count,
        };
    }

    public async Task<BusinessActionResult<Codebook>> AddCodebookAsync(CodebookEditModel codebookEditModel)
    {
        var codebookEntity = typeMapper.Map<CodebookEntity>(codebookEditModel);
        await codebookRepository.AddAsync(codebookEntity);
        codebookRepository.UnitOfWork.Commit();

        return typeMapper.Map<Codebook>(codebookEntity);
    }

    public async Task<BusinessActionResult<Codebook>> GetCodebookAsync(int id)
    {
        var codebookEntity = await codebookRepository.GetAsync(id);

        if (codebookEntity is null)
        {
            return ErrorFactory.NotFound<CodebookEntity>(nameof(CodebookEntity.Id), id);
        }

        return typeMapper.Map<Codebook>(codebookEntity);
    }

    public async Task<BusinessActionResult<Codebook>> UpdateCodebookAsync(int id, CodebookEditModel codebookEditModel)
    {
        var codebookEntity = await codebookRepository.GetAsync(id);
        codebookEntity = typeMapper.Map(codebookEditModel, codebookEntity);

        codebookRepository.Update(codebookEntity);
        codebookRepository.UnitOfWork.Commit();

        return typeMapper.Map<Codebook>(codebookEntity);
    }

    public async Task<BusinessActionResult<Codebook>> RemoveCodebookAsync(int id)
    {
        var codebookEntity = await codebookRepository.GetAsync(id);

        if (codebookEntity is null)
        {
            return ErrorFactory.NotFound<CodebookEntity>(nameof(CodebookEntity.Id), id);
        }

        codebookRepository.Remove(codebookEntity);

        return typeMapper.Map<Codebook>(codebookEntity);
    }
}
