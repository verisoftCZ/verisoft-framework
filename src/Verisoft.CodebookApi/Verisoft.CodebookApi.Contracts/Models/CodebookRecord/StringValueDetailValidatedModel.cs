using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.Core.Contracts.Validation;

namespace Verisoft.CodebookApi.Contracts.Models.CodebookRecord;

public class StringValueDetailValidatedModel<TAdditionalProperties, TTenantValue> : StringValueDetailEditModel<TAdditionalProperties, TTenantValue>, IValidatedObject
    where TAdditionalProperties : class
    where TTenantValue : class, ITenantValue
{
    public List<ValidationProblem> ValidationProblems { get; set; } = [];
}