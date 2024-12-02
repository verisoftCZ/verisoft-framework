using System.Security.Principal;

namespace Verisoft.Core.Authentication
{
    public interface IUserContext
    {
        IPrincipal User { get; }

        string GetToken();
    }
}
