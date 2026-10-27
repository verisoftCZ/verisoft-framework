using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Verisoft.Core.Codebooks.Common.Helpers;

public static class MethodInfoHelper
{
    public static readonly MethodInfo EfPropertyMethod =
        typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Public | BindingFlags.Static)
                  ?.MakeGenericMethod(typeof(string))
        ?? throw new InvalidOperationException("Can't find method EF.Property<string>.");

    public static readonly MethodInfo EnumerableContainsMethod =
        typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                          .FirstOrDefault(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                          ?.MakeGenericMethod(typeof(string))
        ?? throw new InvalidOperationException("Can't find method Enumerable.Contains<string>.");

    public static readonly MethodInfo StringContainsMethod =
        typeof(string).GetMethod("Contains", [typeof(string)])
        ?? throw new InvalidOperationException("Can't find method string.Contains(string).");

    public static readonly MethodInfo EnumerableAnyMethodDefinition =
        typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                          .FirstOrDefault(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2)
        ?? throw new InvalidOperationException("Can't find definition for Enumerable.Any<TSource>(IEnumerable<TSource>, Func<TSource, bool>).");

    public static readonly MethodInfo QueryableAsQueryableMethodDefinition =
        typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                         .FirstOrDefault(m => m.Name == nameof(Queryable.AsQueryable) && m.IsGenericMethodDefinition && m.GetParameters().Length == 1)
        ?? throw new InvalidOperationException("Can't find definition for Queryable.AsQueryable<TElement>(IEnumerable<TElement>).");

    public static MethodInfo GetQueryableWhereMethodWithExpression => GetQueryableMethodWithExpression("Where", 1);

    public static MethodInfo GetQueryableSelectMethodWithExpression => GetQueryableMethodWithExpression("Select", 2);

    public static MethodInfo GetQueryableFirstOrDefaultMethod(int methodGenericArgsCount)
    {
        return typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                   .FirstOrDefault(m => m.Name == "FirstOrDefault" &&
                                       m.IsGenericMethodDefinition &&
                                       m.GetGenericArguments().Length == methodGenericArgsCount &&
                                       m.GetParameters().Length == 1)
                   ?? throw new InvalidOperationException($"Can't find method Queryable.FirstOrDefault with {methodGenericArgsCount} generic arguments and 1 parameter.");
    }

    private static MethodInfo GetQueryableMethodWithExpression(string methodName, int methodGenericArgsCount)
    {
        var methods = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m => m.Name == methodName &&
                        m.IsGenericMethodDefinition &&
                        m.GetGenericArguments().Length == methodGenericArgsCount &&
                        m.GetParameters().Length == 2);

        foreach (var m in methods)
        {
            var secondParamType = m.GetParameters()[1].ParameterType;
            if (secondParamType.IsGenericType &&
                secondParamType.GetGenericTypeDefinition() == typeof(Expression<>))
            {
                var expressionDelegateType = secondParamType.GetGenericArguments()[0];
                if (expressionDelegateType.IsGenericType &&
                    expressionDelegateType.GetGenericTypeDefinition() == typeof(Func<,>))
                {
                    if (methodName == "Where" && expressionDelegateType.GetGenericArguments()[1] == typeof(bool))
                    {
                        return m;
                    }
                    else if (methodName == "Select")
                    {
                        if (expressionDelegateType.GetGenericArguments().Length == 2)
                        {
                            return m;
                        }
                    }
                }
            }
        }

        throw new InvalidOperationException($"Can't find suitable method Queryable.{methodName} with Expression parameter and {methodGenericArgsCount} generic arguments.");
    }
}