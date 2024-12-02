/*
MIT License
Copyright (c) 2024 David Fowler

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Original source: https://github.com/davidfowl/WaitForDependenciesAspire
*/

using System.Diagnostics;
using Aspire.Hosting.ApplicationModel;
using HealthChecks.SqlServer;

namespace Verisoft.Core.Aspire.Hosting.WaitFor;

public static class SqlResourceHealthCheckExtensions
{
    /// <summary>
    /// Adds a health check to the SQL Server server resource.
    /// </summary>
    public static IResourceBuilder<SqlServerServerResource> WithHealthCheck(this IResourceBuilder<SqlServerServerResource> builder)
    {
        return builder.WithSqlHealthCheck(cs => new SqlServerHealthCheckOptions { ConnectionString = cs });
    }

    /// <summary>
    /// Adds a health check to the SQL Server database resource.
    /// </summary>
    public static IResourceBuilder<SqlServerDatabaseResource> WithHealthCheck(this IResourceBuilder<SqlServerDatabaseResource> builder)
    {
        return builder.WithSqlHealthCheck(cs => new SqlServerHealthCheckOptions { ConnectionString = cs });
    }

    /// <summary>
    /// Adds a health check to the SQL Server server resource with a specific query.
    /// </summary>
    public static IResourceBuilder<SqlServerServerResource> WithHealthCheck(this IResourceBuilder<SqlServerServerResource> builder, string query)
    {
        return builder.WithSqlHealthCheck(cs => new SqlServerHealthCheckOptions { ConnectionString = cs, CommandText = query });
    }

    /// <summary>
    /// Adds a health check to the SQL Server database resource  with a specific query.
    /// </summary>
    public static IResourceBuilder<SqlServerDatabaseResource> WithHealthCheck(this IResourceBuilder<SqlServerDatabaseResource> builder, string query)
    {
        return builder.WithSqlHealthCheck(cs => new SqlServerHealthCheckOptions { ConnectionString = cs, CommandText = query });
    }

    private static IResourceBuilder<T> WithSqlHealthCheck<T>(this IResourceBuilder<T> builder, Func<string, SqlServerHealthCheckOptions> healthCheckOptionsFactory)
        where T : IResource
    {
        return builder.WithAnnotation(
            HealthCheckAnnotation.Create(
                cs =>
                {
                    var opt = healthCheckOptionsFactory.Invoke(cs);
                    Debug.WriteLine($"CS: {opt.ConnectionString}");
                    return new SqlServerHealthCheck(healthCheckOptionsFactory(cs));
                }
            )
        );
    }
}
