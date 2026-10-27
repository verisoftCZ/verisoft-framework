using System.Linq.Expressions;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.Core.Codebooks.Common.Specifications;

public class StringValueDetailSpecification<TCodebookRecordType> : ISpecification<CodebookRecordEntity<TCodebookRecordType>>
    where TCodebookRecordType : CodebookRecordType, new()
{
    public string? SearchTerm { get; set; }

    public Expression<Func<CodebookRecordEntity<TCodebookRecordType>, bool>> SatisfiedBy()
    {
        Specification<CodebookRecordEntity<TCodebookRecordType>> specification = new TrueSpecification<CodebookRecordEntity<TCodebookRecordType>>();
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var upperSearchTerm = SearchTerm.ToUpper();
            var searchTermSpecifications = new List<DirectSpecification<CodebookRecordEntity<TCodebookRecordType>>>
            {
                new(x => x.StringValue.ToUpper().Contains(upperSearchTerm)),
                new(x => x.Description != null && x.Description.ToUpper().Contains(upperSearchTerm)),
            };

            specification = SpecificationExtender.OrAll([.. searchTermSpecifications]);
        }

        return specification.SatisfiedBy();
    }
}