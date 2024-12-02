namespace Verisoft.Core.Contracts.BusinessResult;

public class BusinessActionError
{
    internal BusinessActionError(string code, string message, object parameters = null)
    {
        Code = code ?? string.Empty;
        Message = message;
        Parameters = parameters;
    }

    public string Code { get; }

    public string Message { get; }

    public object Parameters { get; }
}
