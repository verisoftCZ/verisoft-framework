using System;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public sealed class AndSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {
        private readonly ISpecification<T> rightSideSpecification;
        private readonly ISpecification<T> leftSideSpecification;

        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            leftSideSpecification = leftSide ?? throw new ArgumentNullException(nameof(leftSide));
            rightSideSpecification = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
        }

        public override ISpecification<T> LeftSideSpecification => leftSideSpecification;

        public override ISpecification<T> RightSideSpecification => rightSideSpecification;

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = leftSideSpecification.SatisfiedBy();
            var right = rightSideSpecification.SatisfiedBy();
            return left.And(right);
        }
    }
}