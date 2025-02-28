using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Models.Translation;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.CodebookApi.Application.Services;

public class TranslationService(ITranslationRepository translationRepository, ILanguageRepository languageRepository, ITypeMapper typeMapper) : ITranslationService
{
    private readonly ITranslationRepository translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
    private readonly ILanguageRepository languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
    private readonly ITypeMapper typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));

    public async Task<StringValueTranslations> GetStringValueTranslationsAsync(string tableName, string stringValue)
    {
        var translationEntities = await translationRepository.GetTranslationsAsync(stringValue, tableName);
        return new StringValueTranslations
        {
            Translations = typeMapper.Map<Translation>(translationEntities),
        };
    }

    public async Task<BusinessActionResult<StringValueTranslations>> UpdateStringValueTranslationsAsync(string tableName, string stringValue, StringValueTranslations translations)
    {
        var translationEntities = await translationRepository.GetTranslationsAsync(stringValue, tableName);

        var translationsToUpdate = translationEntities.Join(translations.Translations, x => x.LanguageId, y => y.LanguageId, (entity, model) => (entity, model)).Where(x => x.entity.Value != x.model.Value).ToList();
        var translationsToCreate = translations.Translations.ExceptBy(translationEntities.Select(x => x.LanguageId), t => t.LanguageId).ToList();
        var translationsToDelete = translationEntities.ExceptBy(translations.Translations.Select(x => x.LanguageId), t => t.LanguageId).ToList();
        var languageEntities = await languageRepository.GetAll();
        foreach (var (entity, model) in translationsToUpdate)
        {
            entity.Value = model.Value;
            await translationRepository.UpdateAsync(entity);
        }

        foreach (var model in translationsToCreate)
        {
            var entity = new TranslationEntity
            {
                Language = languageEntities.Single(x => x.Id == model.LanguageId),
                LanguageId = model.LanguageId,
                TableStringValue = stringValue,
                TableName = tableName,
                Value = model.Value,
            };
            await translationRepository.AddAsync(entity);
        }

        foreach (var entity in translationsToDelete)
        {
            await translationRepository.RemoveAsync(entity);
        }

        translationRepository.UnitOfWork.Commit();

        translationEntities = await translationRepository.GetTranslationsAsync(stringValue, tableName);

        return new StringValueTranslations
        {
            Translations = typeMapper.Map<Translation>(translationEntities),
        };
    }
}