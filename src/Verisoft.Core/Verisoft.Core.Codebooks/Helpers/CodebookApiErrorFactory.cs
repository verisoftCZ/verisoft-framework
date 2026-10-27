using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Helpers;

public class CodebookApiErrorFactory : ErrorFactory
{
    private const string ForeignKeyConstraintViolationMessage = "This codebook record is used by another entity and cannot be deleted. If you want to restrict it's usage, set Valid To instead.";

    internal static BusinessActionError ForeignKeyConstraintViolation() => Error(nameof(ForeignKeyConstraintViolationMessage), ForeignKeyConstraintViolationMessage);
}
