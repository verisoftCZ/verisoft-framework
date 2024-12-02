namespace Verisoft.Core.Contracts.Validations;

public class ValidationError
{
    public string PropertyName { get; set; }

    public string ErrorMessage { get; set; }

    public string ErrorCode { get; set; }
}
