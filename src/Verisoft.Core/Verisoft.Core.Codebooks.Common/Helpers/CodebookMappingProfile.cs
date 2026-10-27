using AutoMapper;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Common.Specifications;

namespace Verisoft.Core.Codebooks.Common.Helpers;

public class CodebookMappingProfile : Profile
{
    public CodebookMappingProfile()
    {
        CreateMap<CodebookEntity, Codebook>();
        CreateMap<CodebookEditModel, CodebookEntity>();
        CreateMap<CodebookFilter, CodebookSpecification>();
        CreateMap<CodebookEditModel, CodebookValidatedModel>();

        CreateMap<LanguageEntity, Language>();
        CreateMap<LanguageEditModel, LanguageEntity>();
        CreateMap<LanguageEditModel, LanguageValidatedModel>();
    }
}