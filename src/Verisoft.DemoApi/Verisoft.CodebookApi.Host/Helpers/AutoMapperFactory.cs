using AutoMapper;
using Verisoft.Core.Codebooks.Common.Helpers;

namespace Verisoft.CodebookApi.Host.Helpers;

public static class AutoMapperFactory
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
            cfg.AddProfile<CodebookMappingProfile>();
        });
    }
}