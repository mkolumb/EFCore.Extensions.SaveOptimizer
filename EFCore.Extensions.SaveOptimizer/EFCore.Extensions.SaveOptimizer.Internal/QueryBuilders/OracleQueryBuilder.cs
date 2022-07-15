using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public class OracleQueryBuilder : BaseQueryBuilder
{
    private static readonly Dictionary<ClauseType, string> Clauses = new()
    {
        { ClauseType.Insert, "INSERT INTO" },
        { ClauseType.Update, "UPDATE" },
        { ClauseType.Delete, "DELETE FROM" },
        { ClauseType.Values, "" },
        { ClauseType.ValuesOne, "VALUES " },
        { ClauseType.ValueEscapeLeft, "\"" },
        { ClauseType.ValueEscapeRight, "\"" },
        { ClauseType.TableEscape, "\".\"" },
        { ClauseType.ParameterPrefix, ":p" },
        { ClauseType.ValueSetLeft, "SELECT " },
        { ClauseType.ValueSetRight, " FROM DUAL UNION ALL " },
        { ClauseType.ValueSetRightLast, " FROM DUAL" },
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
        { ClauseType.Null, "NULL" }
    };

    public OracleQueryBuilder(QueryBuilderConfiguration? configuration) : base(Clauses, configuration)
    {
    }

    protected override string CastParameter(string paramKey, SqlValueModel model)
    {
        if (model.PropertyTypeModel.ColumnType == null)
        {
            return paramKey;
        }

        return paramKey == ClausesConfiguration[ClauseType.Null]
            ? $"CAST({ClausesConfiguration[ClauseType.Null]} AS {model.PropertyTypeModel.ColumnType})"
            : paramKey;
    }
}
