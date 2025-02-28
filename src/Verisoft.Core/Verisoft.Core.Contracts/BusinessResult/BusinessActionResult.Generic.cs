using Verisoft.Core.Contracts.Validation;
using Verisoft.Core.Contracts.Validation.Extensions;

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
            var highestSeverityError = validatedObject.GetHighestSeverityValidationError();

            return highestSeverityError.Code switch
            {
                ErrorFactory.NullInputErrorCode => new BusinessActionResult<TResult>(ErrorFactory.NullInput(highestSeverityError.PropertyName)),
                ErrorFactory.NotFoundErrorCode => new BusinessActionResult<TResult>(ErrorFactory.NotFound(highestSeverityError.TryGetAttemptedValueValue("entityTypeName"), highestSeverityError.PropertyName, highestSeverityError.TryGetAttemptedValueValue("propertyValue"))),
                _ => new BusinessActionResult<TResult>(result, ErrorFactory.UnprocessableEntity(validatedObject.ValidationProblems)),
            };
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