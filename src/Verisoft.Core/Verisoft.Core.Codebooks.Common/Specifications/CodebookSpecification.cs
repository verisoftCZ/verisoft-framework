using System.Linq.Expressions;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.Core.Codebooks.Common.Specifications;

public class CodebookSpecification : ISpecification<CodebookEntity>
{
    public string SearchField { get; set; } = string.Empty;

    public bool? IsExternal { get; set; }

    public Expression<Func<CodebookEntity, bool>> SatisfiedBy()
    {
        Specification<CodebookEntity> specification = new TrueSpecification<CodebookEntity>();

        if (!string.IsNullOrWhiteSpace(SearchField))
        {
            var upperSearchField = SearchField.ToUpper();
            var nameSpecification = new DirectSpecification<CodebookEntity>(c => c.Name.ToUpper().Contains(upperSearchField));
            var codeSpecification = new DirectSpecification<CodebookEntity>(c => c.Code.ToUpper().Contains(upperSearchField));

            specification = SpecificationExtender.OrAll(
                nameSpecification,
                codeSpecification);
        }

        if (IsExternal.HasValue)
        {
            specification &= new DirectSpecification<CodebookEntity>(c => c.IsExternal == IsExternal.Value);
        }

        return specification.SatisfiedBy();
    }
}
