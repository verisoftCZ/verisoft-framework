using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

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
        var languageEntities = await languageRepository.GetAllAsync();
        return typeMapper.Map<Language>(languageEntities);
    }

    public async Task<BusinessActionResult<Language>> UpdateLanguageAsync(LanguageEditModel languageEditModel, int id)
    {
        var languageEntity = await languageRepository.GetAsync(id);
        languageEntity = typeMapper.Map(languageEditModel, languageEntity);
        languageRepository.Update(languageEntity);
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

        languageRepository.Remove(languageEntity);
        languageRepository.UnitOfWork.Commit();

        return typeMapper.Map<Language>(languageEntity);
    }
}
