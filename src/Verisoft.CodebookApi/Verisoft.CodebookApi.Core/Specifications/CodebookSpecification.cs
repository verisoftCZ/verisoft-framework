using System.Linq.Expressions;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.CodebookApi.Core.Specifications
{
    public class CodebookSpecification : ISpecification<CodebookEntity>
    {
        public string? Name { get; set; }

        public bool? IsBasicType { get; set; }

        public bool? HasHardcodedEnum { get; set; }

        public Expression<Func<CodebookEntity, bool>> SatisfiedBy()
        {
            Specification<CodebookEntity> specification = new TrueSpecification<CodebookEntity>();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                specification = specification &&
                                new DirectSpecification<CodebookEntity>(c =>
                                 c.Name.Contains(Name));
            }

            if (IsBasicType.HasValue)
            {
                specification = specification &&
                                new DirectSpecification<CodebookEntity>(c =>
                                c.IsBasicType == IsBasicType.Value);
            }

            if (HasHardcodedEnum.HasValue)
            {
                specification = specification &&
                                new DirectSpecification<CodebookEntity>(c =>
                                c.HasHardcodedEnum == HasHardcodedEnum.Value);
            }

            return specification.SatisfiedBy();
        }
    }
}
