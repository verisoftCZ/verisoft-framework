using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Contracts.Validation.Extensions;

public static class ValidatedObjectExtensions
{
    public static void LogError(this IValidatedObject validatedObject, ValidationFailure validationFailure)
    {
        if (validationFailure.ErrorCode == "RemoveFromErrors")
        {
            return;
        }

        validatedObject.ValidationProblems ??= [];

        validatedObject.ValidationProblems.Add(new ValidationProblem
        {
            PropertyName = validationFailure.PropertyName,
            Message = validationFailure.ErrorMessage,
            Code = validationFailure.ErrorCode,
            AttemptedValue = validationFailure.AttemptedValue,
            Severity = validationFailure.Severity,
        });
    }

    public static ValidationProblem GetHighestSeverityValidationError(this IValidatedObject validatedObject)
    {
        var validationErrors = validatedObject.ValidationProblems?.Where(vp => vp.Severity == Severity.Error).ToList() ?? [];
        return validationErrors.OrderByDescending(x => x.Code switch
                               {
                                   ErrorFactory.NullInputErrorCode => 2,
                                   ErrorFactory.NotFoundErrorCode => 1,
                                   _ => 0,
                               }).FirstOrDefault();
    }

    public static bool HasValidationErrors(this IValidatedObject validatedObject)
    {
        return validatedObject.HasValidationProblems(Severity.Error);
    }

    public static bool HasValidationWarnings(this IValidatedObject validatedObject)
    {
        return validatedObject.HasValidationProblems(Severity.Warning);
    }

    public static bool HasValidationInfo(this IValidatedObject validatedObject)
    {
        return validatedObject.HasValidationProblems(Severity.Info);
    }

    public static IActionResult ToActionResult<TValidatedObject>(this TValidatedObject validatedObject)
        where TValidatedObject : IValidatedObject
    {
        BusinessActionResult<TValidatedObject> result = validatedObject;
        return result.ToActionResult();
    }

    private static bool HasValidationProblems(this IValidatedObject validatedObject, Severity severity)
    {
        if (validatedObject is null)
        {
            return false;
        }

        if (validatedObject.ValidationProblems?.Exists(vp => vp.Severity == severity) ?? false)
        {
            return true;
        }

        var properties = validatedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties.Where(prop => typeof(IValidatedObject).IsAssignableFrom(prop.PropertyType)))
        {
            if (property.GetValue(validatedObject) is IValidatedObject nestedValidatedObject && nestedValidatedObject.HasValidationErrors())
            {
                return true;
            }
        }

        return false;
    }
}