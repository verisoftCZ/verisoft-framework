using FluentValidation;

namespace Verisoft.Core.Contracts.Validation;

public class ValidationProblem
{
    public string PropertyName { get; set; }

    public string Message { get; set; }

    public string Code { get; set; }

    public object AttemptedValue { get; set; }

    public Severity Severity { get; set; }
}
