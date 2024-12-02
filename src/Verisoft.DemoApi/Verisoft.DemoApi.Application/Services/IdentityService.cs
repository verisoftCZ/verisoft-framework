using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Verisoft.Core.Contracts;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;

namespace Verisoft.DemoApi.Application.Services;

public class IdentityService(IHttpContextAccessor contextAccessor, IUserRepository userRepository) : IIdentityService
{
    public async Task<UserEntity> GetCurrentUser()
    {
        var identity = contextAccessor.HttpContext?.User;
        var email = identity?.Claims.First(claim => claim.Type == ClaimTypes.Email).Value!;
        var user = await userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new BusinessException($"User with mail: {email} not found");
        }

        return user;
    }
}
