using System;
using System.Linq.Expressions;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Common.Entities;

public sealed record SortClause<T>(
    Expression<Func<T, object>> Expression,
    SortDirection Direction);
