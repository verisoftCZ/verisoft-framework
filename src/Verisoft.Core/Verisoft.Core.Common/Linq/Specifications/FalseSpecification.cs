using System;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public sealed class FalseSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            const bool result = true;

            Expression<Func<TEntity, bool>> trueExpression = t => !result;
            return trueExpression;
        }
    }
}