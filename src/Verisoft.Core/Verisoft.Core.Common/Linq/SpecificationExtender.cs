using System;
using System.Linq.Expressions;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.Core.Common.Linq;

public static class SpecificationExtender
{
    public static Specification<TEntity> BuildOrSpecification<TEntity, TSearchValues>(
        this Specification<TEntity> spec,
        TSearchValues[] searchValues,
        Func<TSearchValues, Expression<Func<TEntity, bool>>> matchingCriteria)
        where TEntity : class
    {
        TSearchValues[] properValues = searchValues.IgnoreEmptyValues();
        if (properValues.Length == 0)
        {
            return spec;
        }

        if (properValues.Length == 1)
        {
            return spec && new DirectSpecification<TEntity>(matchingCriteria(properValues[0]));
        }

        Specification<TEntity> orSpec = new FalseSpecification<TEntity>();
        foreach (var val in properValues)
        {
            orSpec = new OrSpecification<TEntity>(orSpec, new DirectSpecification<TEntity>(matchingCriteria(val)));
        }

        return spec && orSpec;
    }

    public static Specification<TEntity> OrAll<TEntity>(params Specification<TEntity>[] specifications)
        where TEntity : class
    {
        if (specifications == null || specifications.Length == 0)
        {
            throw new ArgumentException("No specifications provided.", nameof(specifications));
        }

        Specification<TEntity> combinedSpecification = null;
        foreach (var spec in specifications)
        {
            if (combinedSpecification == null)
            {
                combinedSpecification = spec;
            }
            else
            {
                combinedSpecification = new OrSpecification<TEntity>(combinedSpecification, spec);
            }
        }

        return combinedSpecification;
    }
}
