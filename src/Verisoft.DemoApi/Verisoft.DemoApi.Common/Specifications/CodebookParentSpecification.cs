using System.Linq.Expressions;
using Verisoft.Core.Codebooks.Common.Extensions;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Entities.CodebookRecords;

namespace Verisoft.DemoApi.Common.Specifications;

public sealed class CodebookParentSpecification : ISpecification<CodebookParentEntity>
{
    public CodebookRecord<Accessibility>[] Accessibility { get; init; } = [];

    public Expression<Func<CodebookParentEntity, bool>> SatisfiedBy()
    {
        Specification<CodebookParentEntity> specification = new TrueSpecification<CodebookParentEntity>();

        if (Accessibility.HasValue())
        {
            specification = specification.BuildAndContainsCodebookSpecification(x => x.Accessibility, Accessibility);
        }

        return specification.SatisfiedBy();
    }
}