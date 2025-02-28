namespace Verisoft.Core.Contracts.Validation.Extensions;

public static class ValidationProblemExtensions
{
    public static string TryGetAttemptedValueValue(this ValidationProblem validationProblem, string propertyName)
    {
        var attemptedValue = validationProblem.AttemptedValue;
        if (attemptedValue == null)
        {
            return null;
        }

        var property = attemptedValue.GetType().GetProperty(propertyName);
        return property != null ? property.GetValue(attemptedValue)?.ToString() : null;
    }
}