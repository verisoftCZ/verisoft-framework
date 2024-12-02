namespace Verisoft.Core.Authentication.MicrosoftIdentity;

public interface ILoginService
{
    Task<string> LoginAsync(string email, string password);
}
