using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Application.Services.Interfaces
{
    public interface IIdentityService
    {
        public Task<UserEntity> GetCurrentUser();
    }
}
