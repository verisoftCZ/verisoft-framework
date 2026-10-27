using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Verisoft.Core.Codebooks.Extensions;

public static class DbExceptionExtensions
{
    private const string PostgresForeignKeyViolation = "23503";
    private const int SqlServerForeignKeyViolation = 547;

    public static bool IsForeignKeyConstraintViolation(this DbUpdateException exception)
    {
        if (exception.InnerException is NpgsqlException npgsqlEx)
        {
            return npgsqlEx.SqlState == PostgresForeignKeyViolation;
        }

        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number == SqlServerForeignKeyViolation;
        }

        return false;
    }

    public static bool IsForeignKeyConstraintViolation(this DbException exception)
    {
        if (exception is NpgsqlException npgsqlEx)
        {
            return npgsqlEx.SqlState == PostgresForeignKeyViolation;
        }

        if (exception is SqlException sqlEx)
        {
            return sqlEx.Number == SqlServerForeignKeyViolation;
        }

        return false;
    }
}
