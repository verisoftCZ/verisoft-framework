namespace Verisoft.DemoApi.Client;

public interface IClientSettings
{
    string ServiceUrl { get; }

    string AuthorizationToken { get; }
}
