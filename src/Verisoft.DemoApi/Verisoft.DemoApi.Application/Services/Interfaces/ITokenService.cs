using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface ITokenService
{
    Task<string> Generate(User user);
}
