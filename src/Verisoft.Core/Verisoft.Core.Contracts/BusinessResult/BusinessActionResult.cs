namespace Verisoft.Core.Contracts.BusinessResult;

public class BusinessActionResult
{
    protected const string SuccessCode = "Success";

    public BusinessActionResult(string code = SuccessCode)
    {
        Code = code;
    }

    public BusinessActionResult(BusinessActionErrors errors)
    {
        if (errors?.HasErrors is null or false)
        {
            throw new System.ArgumentNullException(nameof(errors));
        }

        Errors = errors;
        Code = errors[0].Code;
    }

    public BusinessActionErrors Errors { get; protected set; }

    public string Code { get; set; }

    public bool IsSuccess => Code == SuccessCode;

    public static implicit operator BusinessActionResult(BusinessActionError error)
    {
        return new BusinessActionResult(error);
    }

    public static implicit operator BusinessActionResult(BusinessActionErrors errors)
    {
        return new BusinessActionResult(errors);
    }
}