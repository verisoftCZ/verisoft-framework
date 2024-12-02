using System;
using System.Linq;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public class SubExpressionSpecification<TParent, TChild> : ISpecification<TParent>
        where TParent : class
        where TChild : class
    {
        private readonly Expression<Func<TParent, TChild>> subExpression;

        private readonly ISpecification<TChild> specification;

        public SubExpressionSpecification(Expression<Func<TParent, TChild>> subExpression, ISpecification<TChild> specification)
        {
            this.subExpression = subExpression;
            this.specification = specification;
        }

        public Expression<Func<TParent, bool>> SatisfiedBy()
        {
            var specificationExpression = specification.SatisfiedBy();
            var param = Expression.Parameter(typeof(TParent), "param");

            var newFirst = new ReplaceExpressionVisitor(subExpression.Parameters.First(), param).Visit(subExpression.Body);
            var newSecond = new ReplaceExpressionVisitor(specificationExpression.Parameters.First(), newFirst).Visit(specificationExpression.Body);
            return Expression.Lambda<Func<TParent, bool>>(newSecond, param);
        }
    }
}