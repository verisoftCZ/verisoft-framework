using FluentValidation.Results;
using System.Collections.Generic;
using System.Reflection;

namespace Verisoft.Core.Contracts.Validations.Extensions;

public static class ValidatedObjectExtensions
{
    public static void LogError(this IValidatedObject validatedObject, ValidationFailure validationFailure)
    {
        validatedObject.ValidationErrors ??= new List<ValidationError>();

        validatedObject.ValidationErrors.Add(new ValidationError
        {
            PropertyName = validationFailure.PropertyName,
            ErrorMessage = validationFailure.ErrorMessage,
            ErrorCode = validationFailure.ErrorCode,
        });
    }

    public static bool HasValidationErrors(this IValidatedObject validatedObject)
    {
        if (validatedObject is null)
        {
            return false;
        }

        if (validatedObject.ValidationErrors?.Count > 0)
        {
            return true;
        }

        var properties = validatedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (typeof(IValidatedObject).IsAssignableFrom(property.PropertyType))
            {
                var nestedValidatedObject = property.GetValue(validatedObject) as IValidatedObject;
                if (nestedValidatedObject != null && nestedValidatedObject.HasValidationErrors())
                {
                    return true;
                }
            }
        }

        return false;
    }
}
