using System;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}
