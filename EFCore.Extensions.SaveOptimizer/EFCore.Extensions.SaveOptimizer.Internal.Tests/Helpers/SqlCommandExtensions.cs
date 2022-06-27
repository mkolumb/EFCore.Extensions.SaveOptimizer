using System.Text.RegularExpressions;
using EFCore.Extensions.SaveOptimizer.Internal.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;

public static class SqlCommandExtensions
{
    public static string? CompileSql(this ISqlCommandModel command, Type builderType)
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

        if (builderType == typeof(OracleAllQueryBuilder) &&
            command.GetType() == typeof(SqlKataCommandModel) &&
            sql.Contains("), ("))
        {
            var regex = new Regex("insert (.*) values");

            var match = ") " + regex.Match(sql).Value[7..] + " (";

            sql = sql.Replace("insert into", "insert all into").Replace("), (", match) + " select * from dual";
        }

        if (sql.StartsWith("begin"))
        {
            sql = sql[5..].Trim();
        }

        while (sql.StartsWith(";"))
        {
            sql = sql[1..].Trim();
        }

        if (sql.EndsWith("end"))
        {
            sql = sql[..^3].Trim();
        }

        while (sql.EndsWith(";"))
        {
            sql = sql[..^1].Trim();
        }

        return sql;
    }
}
