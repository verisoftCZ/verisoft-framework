using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Models.Language;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services;

public class LanguageService(ILanguageRepository languageRepository, ITypeMapper typeMapper) : ILanguageService
{
    private readonly ILanguageRepository languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
    private readonly ITypeMapper typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));

    public async Task<BusinessActionResult<Language>> AddLanguageAsync(LanguageEditModel languageEditModel)
    {
        var languageEntity = typeMapper.Map<LanguageEntity>(languageEditModel);
        await languageRepository.AddAsync(languageEntity);
        languageRepository.UnitOfWork.Commit();

        return typeMapper.Map<Language>(languageEntity);
    }

    public async Task<BusinessActionResult<Language>> GetLanguageAsync(int id)
    {
        var languageEntity = await languageRepository.GetAsync(id);

        if (languageEntity is null)
        {
            return ErrorFactory.NotFound<LanguageEntity>(nameof(LanguageEntity.Id), id);
        }

        return typeMapper.Map<Language>(languageEntity);
    }

    public async Task<IEnumerable<Language>> GetLanguagesAsync()
    {
        var languageEntities = await languageRepository.GetAll();
        return typeMapper.Map<Language>(languageEntities);
    }

    public async Task<BusinessActionResult<Language>> UpdateLanguageAsync(LanguageEditModel languageEditModel, int id)
    {
        var languageEntity = await languageRepository.GetAsync(id);
        languageEntity = typeMapper.Map(languageEditModel, languageEntity);
        await languageRepository.UpdateAsync(languageEntity);
        languageRepository.UnitOfWork.Commit();

        return typeMapper.Map<Language>(languageEntity);
    }

    public async Task<BusinessActionResult<Language>> RemoveLanguageAsync(int id)
    {
        var languageEntity = await languageRepository.GetAsync(id);

        if (languageEntity is null)
        {
            return ErrorFactory.NotFound<LanguageEntity>(nameof(LanguageEntity.Id), id);
        }

        await languageRepository.RemoveAsync(languageEntity);
        languageRepository.UnitOfWork.Commit();

        return typeMapper.Map<Language>(languageEntity);
    }
}
