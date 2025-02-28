using Newtonsoft.Json;

namespace Verisoft.Core.Contracts.BusinessResult;

[JsonObject]
public class BusinessActionErrors<TResult>
{
    public BusinessActionErrors(TResult data, BusinessActionErrors errors)
    {
        Data = data;
        Errors = errors;
    }

    public TResult Data { get; private set; }

    public BusinessActionErrors Errors { get; set; }
}
