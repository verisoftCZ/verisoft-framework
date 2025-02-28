using Verisoft.Core.Common.Store;
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

    public async Task<User> GetUserByApiKeyAsync(string apikey)
    {
        var userEntity = await repository.GetUserByApiKey(apikey);
        return typeMapper.Map<User>(userEntity);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var userEntity = await repository.GetUserByEmail(email);
        return typeMapper.Map<User>(userEntity);
    }

    public async Task<UserListData> GetUserListAsync(FilteredRequest<UserFilter> filter)
    {
        var userSpecification = typeMapper.Map<UserSpecification>(filter.Filter);
        var userFilter = userSpecification?.SatisfiedBy();
        var totalItems = await repository.GetCountAsync(userFilter);
        var items = await repository.GetPagedSortAsync<UserSort>(
            filter.Offset,
            filter.Limit,
            userFilter,
            filter.Sort);
        var userData = typeMapper.Map<UserListItem>(items);
        return new UserListData
        {
            Total = totalItems,
            Data = userData,
        };
    }

    public async Task<IEnumerable<User>> GetUsersAsync(FilteredRequest<UserFilter> request)
    {
        var userSpecification = typeMapper.Map<UserSpecification>(request.Filter);
        var filter = userSpecification?.SatisfiedBy();
        var items = await repository.GetPagedSortAsync<UserSort>(
            request.Offset,
            request.Limit,
            filter,
            request.Sort);
        return typeMapper.Map<User>(items).ToList();
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