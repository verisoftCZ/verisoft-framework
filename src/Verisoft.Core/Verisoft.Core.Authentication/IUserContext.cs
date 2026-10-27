using System.Security.Principal;

namespace Verisoft.Core.Authentication
{
    public interface IUserContext
    {
        IPrincipal User { get; }

        string GetUserName();

        string? GetToken();

        void SetUser(IPrincipal user);

        void SetToken(string token);

        void ClearInjectedUserAndToken();
    }
}
