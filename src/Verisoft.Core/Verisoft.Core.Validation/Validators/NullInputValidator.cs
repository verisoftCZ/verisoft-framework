using FluentValidation;
using FluentValidation.Validators;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation;

namespace Verisoft.Core.Validation.Validators;

public class NullInputValidator<TValidatedObject> : PropertyValidator<TValidatedObject, TValidatedObject>
    where TValidatedObject : IValidatedObject
{
    private const string InputModelName = "InputModel";

    public override string Name => "RemoveFromErrors";

    public override bool IsValid(ValidationContext<TValidatedObject> context, TValidatedObject value)
    {
        if (value is null)
        {
            context.AddFailure(new FluentValidation.Results.ValidationFailure
            {
                PropertyName = InputModelName,
                ErrorMessage = ErrorFactory.NullInputErrorMessage,
                ErrorCode = ErrorFactory.NullInputErrorCode,
                AttemptedValue = new { inputName = InputModelName },
            });

            return false;
        }

        return true;
    }
}
