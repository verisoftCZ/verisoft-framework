using Verisoft.Core.Contracts.Validations;
using Verisoft.Core.Contracts.Validations.Extensions;

namespace Verisoft.Core.Contracts.BusinessResult;

public class BusinessActionResult<TResult> : BusinessActionResult
{
    public BusinessActionResult(TResult result, string code = SuccessCode)
    {
        Data = result;
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

    public BusinessActionResult(TResult result, BusinessActionErrors errors)
    {
        if (errors?.HasErrors is null or false)
        {
            throw new System.ArgumentNullException(nameof(errors));
        }

        Data = result;
        Errors = errors;
        Code = errors[0].Code;
    }

    public TResult Data { get; set; }

    public static implicit operator BusinessActionResult<TResult>(TResult result)
    {
        if (result is IValidatedObject validatedObject && validatedObject.HasValidationErrors())
        {
            return new BusinessActionResult<TResult>(result, ErrorFactory.UnprocessableValidated());
        }

        return new BusinessActionResult<TResult>(result);
    }

    public static implicit operator BusinessActionResult<TResult>(BusinessActionError error)
    {
        return new BusinessActionResult<TResult>(error);
    }

    public static implicit operator BusinessActionResult<TResult>(BusinessActionErrors errors)
    {
        return new BusinessActionResult<TResult>(errors);
    }
}