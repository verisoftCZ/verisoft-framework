using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Verisoft.Core.Common.Exceptions;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Common.Entities;

public abstract class BaseSort<T>
    where T : IEntity
{
    [Obsolete($"Use {nameof(DefaultSorts)} instead.")]
    public virtual Expression<Func<T, object>> DefaultSort
        => x => x.Id;

    [Obsolete($"Use {nameof(DefaultSorts)} instead.")]
    public virtual SortDirection DefaultSortDirection
        => SortDirection.Asc;

    public virtual IReadOnlyList<SortClause<T>> DefaultSorts
        => [
            new(DefaultSort, DefaultSortDirection),
        ];

    public virtual Expression<Func<T, object>> GetSortingExpression(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new InvalidSortPropertyException(typeof(T), propertyName);
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = GetNestedPropertyAccess(parameter, propertyName)
            ?? throw new InvalidSortPropertyException(typeof(T), propertyName);

        var converted = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<T, object>>(converted, parameter);
    }

    private static MemberExpression? GetNestedPropertyAccess(ParameterExpression parameter, string propertyPath)
    {
        Expression currentExpression = parameter;

        foreach (var prop in propertyPath.Split('.'))
        {
            var property = currentExpression.Type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
            {
                return null;
            }

            currentExpression = Expression.Property(currentExpression, property);
        }

        return currentExpression as MemberExpression;
    }
}
