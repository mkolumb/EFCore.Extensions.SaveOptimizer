using EFCore.Extensions.SaveOptimizer.Internal.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;

public static class SqlCommandExtensions
{
    public static string? CompileSql(this ISqlCommandModel command)
    {
        var sql = command.Sql;

        if (sql == null)
        {
            return null;
        }

        if (command.NamedBindings == null)
        {
            return sql;
        }

        foreach (var (key, value) in command.NamedBindings)
        {
            sql = sql.Replace(key, SerializationHelper.Serialize(value));
        }

        sql = sql.ToLower();

        while (sql.EndsWith(";"))
        {
            sql = sql[..^1];
        }

        return sql;
    }
}
