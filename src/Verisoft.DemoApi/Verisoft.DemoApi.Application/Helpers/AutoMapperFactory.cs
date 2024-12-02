using AutoMapper;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.Client;
using Verisoft.DemoApi.Contracts.Models.Document;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Application.Helpers;

public static class AutoMapperFactory
{
    public static IMapper CreateMapper()
    {
        MapperConfiguration config = CreateConfig();
        return config.CreateMapper();
    }

    private static MapperConfiguration CreateConfig()
    {
        return new MapperConfiguration(cfg =>
        {
            MapClient(cfg);
            MapUser(cfg);
            MapDocument(cfg);
        });
    }

    private static void MapClient(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ClientFilter, ClientSpecification>();
        cfg.CreateMap<ClientEntity, Client>();
        cfg.CreateMap<ClientEntity, ClientListItem>();
        cfg.CreateMap<ClientEditModel, ClientEntity>();
    }

    private static void MapUser(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<UserFilter, UserSpecification>();
        cfg.CreateMap<UserEntity, User>();
        cfg.CreateMap<UserEntity, UserListItem>();
        cfg.CreateMap<UserEditModel, UserEntity>();
        cfg.CreateMap<UserCreateModel, UserEntity>();
    }

    private static void MapDocument(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<DocumentFilter, DocumentSpecification>();
        cfg.CreateMap<DocumentEntity, Document>();
        cfg.CreateMap<DocumentCreateModel, DocumentEntity>();
    }
}