using System.Security.Principal;
using Verisoft.Core.Authentication;

namespace Verisoft.Core.Data.EntityFramework.Test.Walkers.TestInfrastructure;

public class StubUserContext : IUserContext
{
    public IPrincipal User => new GenericPrincipal(new GenericIdentity("test-user"), null);

    public string GetUserName() => "test-user";

    public string? GetToken() => null;

    public void SetUser(IPrincipal user) { }

    public void SetToken(string token) { }

    public void ClearInjectedUserAndToken() { }
}
