namespace Verisoft.DemoApi.Host.Exceptions;

public class DemoException : Exception
{
    public DemoException()
    {
    }

    public DemoException(string message)
        : base(message)
    {
    }

    public DemoException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
