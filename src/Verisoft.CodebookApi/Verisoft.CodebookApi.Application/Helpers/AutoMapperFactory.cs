using AutoMapper;
using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Codebook;
using Verisoft.CodebookApi.Contracts.Models.Language;
using Verisoft.CodebookApi.Contracts.Models.Translation;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Specifications;

namespace Verisoft.CodebookApi.Application.Helpers;

public class AutoMapperFactory
{
    public static IMapper CreateMapper()
    {
        var config = CreateConfig();
        return config.CreateMapper();
    }

    public static MapperConfiguration CreateConfig()
    {
        return new MapperConfiguration(cfg =>
        {
            MapCodebook(cfg);
            MapLanguage(cfg);
            MapTranslation(cfg);
        });
    }

    private static void MapCodebook(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<CodebookEntity, Codebook>();
        cfg.CreateMap<CodebookEditModel, CodebookEntity>();
        cfg.CreateMap<CodebookFilter, CodebookSpecification>();
        cfg.CreateMap<CodebookEditModel, CodebookValidatedModel>();
    }

    private static void MapLanguage(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<LanguageEntity, Language>();
        cfg.CreateMap<LanguageEditModel, LanguageEntity>();
        cfg.CreateMap<LanguageEditModel, LanguageValidatedModel>();
    }

    private static void MapTranslation(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<TranslationEntity, Translation>();
        cfg.CreateMap<Translation, TranslationValidatedModel>();
        cfg.CreateMap<StringValueTranslations, StringValueTranslationsValidatedModel>();
    }
}