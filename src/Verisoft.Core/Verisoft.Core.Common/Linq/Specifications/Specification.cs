﻿using System;
using System.Linq.Expressions;

namespace Verisoft.Core.Common.Linq.Specifications
{
    public abstract class Specification<TEntity>
        : ISpecification<TEntity>
        where TEntity : class
    {
        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }

        public static bool operator true(Specification<TEntity> specification)
        {
            return true;
        }

        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}