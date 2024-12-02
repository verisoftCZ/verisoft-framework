using System;
using System.Linq;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public sealed class NotSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> originalCriteria;

        public NotSpecification(ISpecification<TEntity> originalSpecification)
        {
            if (originalSpecification is null)
            {
                throw new ArgumentNullException(nameof(originalSpecification));
            }

            originalCriteria = originalSpecification.SatisfiedBy();
        }

        public NotSpecification(Expression<Func<TEntity, bool>> originalSpecification)
        {
            originalCriteria = originalSpecification ?? throw new ArgumentNullException(nameof(originalSpecification));
        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Not(originalCriteria.Body),
                originalCriteria.Parameters.Single());
        }
    }
}