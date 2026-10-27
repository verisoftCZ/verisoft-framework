using Verisoft.Core.Common.TypeMapper;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Common.Sorts;
using Verisoft.DemoApi.Common.Specifications;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Application.Services;

public class UserService(IUserRepository repository, ITypeMapper typeMapper) : IUserService
{
    public async Task<BusinessActionResult<User>> AddUserAsync(UserCreateModel userCreateModel)
    {
        if (userCreateModel is null)
        {
            return ErrorFactory.NullInput(nameof(userCreateModel));
        }

        var userEntity = typeMapper.Map<UserEntity>(userCreateModel);
        userEntity.PasswordHash = "hash";
        userEntity.Salt = "salt";
        await repository.AddAsync(userEntity);
        repository.UnitOfWork.Commit();
        return typeMapper.Map<User>(userEntity);
    }

    public async Task<BusinessActionResult<User>> GetUserAsync(int id)
    {
        var userEntity = await repository.GetAsync(id);
        if (userEntity is null)
        {
            return ErrorFactory.NotFound<UserEntity>(nameof(UserEntity.Id), id);
        }

        return typeMapper.Map<User>(userEntity);
    }

    public async Task<User?> GetUserByApiKeyAsync(string apikey)
    {
        var userEntity = await repository.GetUserByApiKey(apikey);
        if (userEntity is not null)
        {
            return typeMapper.Map<User>(userEntity);
        }

        return null;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var userEntity = await repository.GetUserByEmail(email);
        if (userEntity is not null)
        {
            return typeMapper.Map<User>(userEntity);
        }

        return null;
    }

    public async Task<UserListData?> GetUserListAsync(FilteredRequest<UserFilter> filter)
    {
        var userSpecification = typeMapper.Map<UserSpecification>(filter.Filter);
        var userFilter = userSpecification?.SatisfiedBy();
        var items = await repository.GetPagedSortAsync<UserSort>(
            filter.Offset,
            filter.Limit,
            userFilter,
            filter.ParsedSorts);
        var userData = typeMapper.Map<UserListItem>(items.Items);
        return new UserListData
        {
            Total = items.Count,
            Data = userData,
        };
    }

    public async Task<IEnumerable<User>?> GetUsersAsync(FilteredRequest<UserFilter> request)
    {
        var userSpecification = typeMapper.Map<UserSpecification>(request.Filter);
        var filter = userSpecification.SatisfiedBy();
        if (filter is not null)
        {
            var items = await repository.GetPagedSortAsync<UserSort>(
                request.Offset,
                request.Limit,
                filter,
                request.ParsedSorts);
            return typeMapper.Map<User>(items.Items).ToList();
        }

        return null;
    }

    public async Task<BusinessActionResult<User>> RemoveUserAsync(int id)
    {
        var userEntity = await repository.GetAsync(id);
        if (userEntity is null)
        {
            return ErrorFactory.NotFound<UserEntity>(nameof(UserEntity.Id), id);
        }

        repository.Remove(userEntity);
        repository.UnitOfWork.Commit();
        return typeMapper.Map<User>(userEntity);
    }

    public async Task<BusinessActionResult<User>> UpdateUserAsync(UserEditModel userEditModel, int id)
    {
        if (userEditModel is null)
        {
            return ErrorFactory.NullInput(nameof(userEditModel));
        }

        var userEntity = await repository.GetAsync(id);
        if (userEntity is null)
        {
            return ErrorFactory.NotFound<UserEntity>(nameof(UserEntity.Id), id);
        }

        userEntity = typeMapper.Map(userEditModel, userEntity);
        repository.Update(userEntity);
        repository.UnitOfWork.Commit();
        return typeMapper.Map<User>(userEntity);
    }
}