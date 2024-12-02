namespace Verisoft.Core.Contracts.BusinessResult;

public class BusinessActionErrors<TResult> : BusinessActionErrors
{
    public BusinessActionErrors(TResult data, BusinessActionErrors errors)
    {
        Data = data;
        AddRange(errors);
    }

    public TResult Data { get; }
}
