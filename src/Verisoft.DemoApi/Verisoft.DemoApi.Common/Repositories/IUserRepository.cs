using Verisoft.Core.Common.Store;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Repositories;

public interface IUserRepository : IRepository<UserEntity, int>
{
    Task<UserEntity> GetUserByApiKey(string apikey);

    Task<UserEntity> GetUserByEmail(string email);
}