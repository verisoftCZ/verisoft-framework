using System;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public sealed class OrSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            LeftSideSpecification = leftSide ?? throw new ArgumentNullException(nameof(leftSide));
            RightSideSpecification = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
        }

        public override ISpecification<T> LeftSideSpecification { get; }

        public override ISpecification<T> RightSideSpecification { get; }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = LeftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = RightSideSpecification.SatisfiedBy();
            return left.Or(right);
        }
    }
}