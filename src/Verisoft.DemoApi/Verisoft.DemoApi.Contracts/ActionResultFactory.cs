using Microsoft.AspNetCore.Http;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.DemoApi.Contracts;

public class ActionResultFactory : ErrorFactory
{
    private const string RequestedExportTypeIsNotSupported = "The requested media type is not supported";

    public static BusinessActionError UnsupportedMediaType(string mediaType)
    {
        return Error(StatusCodes.Status415UnsupportedMediaType.ToString(),
            RequestedExportTypeIsNotSupported,
            new { mediaType });
    }
}