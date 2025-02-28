namespace Verisoft.CodebookApi.Client;

public interface IClientSettings
{
    string ServiceUrl { get; }

    string AuthorizationToken { get; }
}
