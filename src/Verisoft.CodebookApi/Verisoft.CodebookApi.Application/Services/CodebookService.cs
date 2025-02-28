using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.Codebook;
using Verisoft.CodebookApi.Core.Configurations;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Core.Sorts;
using Verisoft.CodebookApi.Core.Specifications;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services;

public class CodebookService(ICodebookRepository codebookRepository, ITypeMapper typeMapper, IMainCodebookRecordConfiguration mainCodebookRecordConfiguration) : ICodebookService
{
    private readonly ICodebookRepository codebookRepository = codebookRepository ?? throw new ArgumentNullException(nameof(codebookRepository));
    private readonly ITypeMapper typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
    private readonly IMainCodebookRecordConfiguration mainCodebookRecordConfiguration = mainCodebookRecordConfiguration ?? throw new ArgumentNullException(nameof(mainCodebookRecordConfiguration));

    public async Task<BusinessActionResult<CodebookListData>> GetCodebooksAsync(FilteredRequest<CodebookFilter> request)
    {
        request.Filter ??= new();
        var specification = typeMapper.Map<CodebookSpecification>(request.Filter);
        var filter = specification.SatisfiedBy();
        var totalItems = await codebookRepository.GetCountAsync(filter);
        var entities = await codebookRepository.GetPagedSortAsync<CodebookSort>(request.Offset, request.Limit, filter, request.Sort);

        return new CodebookListData
        {
            Data = typeMapper.Map<Codebook>(entities),
            Total = totalItems,
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

        return MapEntityToContract(codebookEntity);
    }

    public async Task<BusinessActionResult<Codebook>> UpdateCodebookAsync(int id, CodebookEditModel codebookEditModel)
    {
        var codebookEntity = await codebookRepository.GetAsync(id);
        codebookEntity = typeMapper.Map(codebookEditModel, codebookEntity);

        await codebookRepository.UpdateAsync(codebookEntity);
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

        await codebookRepository.RemoveAsync(codebookEntity);
        codebookRepository.UnitOfWork.Commit();

        return typeMapper.Map<Codebook>(codebookEntity);
    }

    private Codebook MapEntityToContract(CodebookEntity? codebookEntity)
    {
        var contract = typeMapper.Map<Codebook>(codebookEntity);
        var codebookConfig = mainCodebookRecordConfiguration.CodebookRecordConfiguration.FirstOrDefault(x => x.CodebookRecordTableName == contract.Name);
        if (codebookConfig is not null && codebookConfig.PropContract != typeof(BasicDefaultContract))
        {
            var properties = codebookConfig.PropContract.GetProperties();
            contract.AdditionalProperties = properties.ToDictionary(x => x.Name, x => x.PropertyType.Name);
        }

        return contract;
    }
}
