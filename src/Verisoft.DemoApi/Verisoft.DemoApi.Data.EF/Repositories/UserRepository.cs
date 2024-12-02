using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Authentication;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class UserRepository(IDemoApiDbContext unitOfWork, IUserContext userContext, ILogger<UserRepository> logger)
    : BaseRepository<UserEntity, int>(unitOfWork, userContext, logger), IUserRepository
{
    public async Task<UserEntity> GetUserById(int id)
    {
        return await GetAsync(id);
    }

    public async Task<UserEntity> GetUserByEmail(string email)
    {
        var user = await GetFilteredAsync(x => x.Email.ToLower() == email.ToLower());
        return user.FirstOrDefault();
    }

    public async Task<UserEntity> GetUserByApiKey(string apikey)
    {
        var user = await GetFilteredAsync(x => x.ApiKey == apikey);
        return user.FirstOrDefault();
    }

    protected override DbSet<UserEntity> GetDbSet()
    {
        return Context.User;
    }
}