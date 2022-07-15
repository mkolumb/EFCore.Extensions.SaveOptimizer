using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public class OracleAllQueryBuilder : BaseQueryBuilder
{
    private static readonly Dictionary<ClauseType, string> Clauses = new()
    {
        { ClauseType.Insert, "INSERT INTO" },
        { ClauseType.InsertAll, "INSERT ALL" },
        { ClauseType.Into, "INTO" },
        { ClauseType.Update, "UPDATE" },
        { ClauseType.Delete, "DELETE FROM" },
        { ClauseType.Values, "VALUES " },
        { ClauseType.ValuesOne, "VALUES " },
        { ClauseType.ValueEscapeLeft, "\"" },
        { ClauseType.ValueEscapeRight, "\"" },
        { ClauseType.TableEscape, "\".\"" },
        { ClauseType.ParameterPrefix, ":p" },
        { ClauseType.ValueSetLeft, "(" },
        { ClauseType.ValueSetRight, ")" },
        { ClauseType.ValueSetRightLast, "" },
        { ClauseType.ValueSetOneLeft, "(" },
        { ClauseType.ValueSetOneRight, ")" },
        { ClauseType.ValueSetSeparator, "" },
        { ClauseType.Where, "WHERE" },
        { ClauseType.In, "IN" },
        { ClauseType.Or, "OR" },
        { ClauseType.And, "AND" },
        { ClauseType.Set, "SET" },
        { ClauseType.RangeLeft, "(" },
        { ClauseType.RangeRight, ")" },
        { ClauseType.QueryEnding, "" },
        { ClauseType.QueryAppendix, " SELECT * FROM DUAL" }
    };

    private bool _insertAll;

    public OracleAllQueryBuilder(QueryBuilderConfiguration? configuration) : base(Clauses, configuration)
    {
    }

    public override IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, SqlValueModel?>> data)
    {
        IQueryBuilder result = data.Count == 1 ? base.Insert(tableName, data) : InsertAll(tableName, data);

        return result;
    }

    private IQueryBuilder InsertAll(string tableName, IReadOnlyList<IDictionary<string, SqlValueModel?>> data)
    {
        _insertAll = true;

        Builder.Append($"{ClausesConfiguration[ClauseType.InsertAll]}");

        var idx = 0;

        StringBuilder columnBuilder = new();

        columnBuilder.Append($" {ClausesConfiguration[ClauseType.Into]} {GetTableName(tableName)} (");

        foreach (var key in data[0].Keys)
        {
            columnBuilder.Append(idx > 0
                ? $", {ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]}"
                : $"{ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]}");

            idx++;
        }

        columnBuilder.Append($") {ClausesConfiguration[ClauseType.Values]}");

        for (var i = 0; i < data.Count; i++)
        {
            Builder.Append(columnBuilder);

            var valueSetLeft = ClausesConfiguration[ClauseType.ValueSetLeft];
            var valueSetRight = ClausesConfiguration[ClauseType.ValueSetRight];

            Builder.Append(i > 0
                ? $"{ClausesConfiguration[ClauseType.ValueSetSeparator]}{valueSetLeft}"
                : valueSetLeft);

            idx = 0;

            foreach (var key in data[i].Keys)
            {
                AppendValueIn(data[i][key], idx > 0);

                idx++;
            }

            Builder.Append(valueSetRight);
        }

        Builder.Append(ClausesConfiguration[ClauseType.QueryAppendix]);

        return this;
    }

    public override ISqlCommandModel Build()
    {
        ISqlCommandModel command = base.Build();

        if (!_insertAll)
        {
            return command;
        }

        var sql = $"BEGIN{Environment.NewLine}{command.Sql};{Environment.NewLine}END;";

        return new SqlCommandModel(sql, command.Parameters);
    }
}
