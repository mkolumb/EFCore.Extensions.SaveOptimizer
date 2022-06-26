using EFCore.Extensions.SaveOptimizer.Internal.Enums;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public class SqliteQueryBuilder : BaseQueryBuilder
{
    private static readonly Dictionary<ClauseType, string> Clauses = new()
    {
        { ClauseType.Insert, "INSERT INTO" },
        { ClauseType.Update, "UPDATE" },
        { ClauseType.Delete, "DELETE FROM" },
        { ClauseType.Values, "VALUES " },
        { ClauseType.ValuesOne, "VALUES " },
        { ClauseType.ValueEscapeLeft, "\"" },
        { ClauseType.ValueEscapeRight, "\"" },
        { ClauseType.TableEscape, "\".\"" },
        { ClauseType.ParameterPrefix, "@p" },
        { ClauseType.ValueSetLeft, "(" },
        { ClauseType.ValueSetRight, ")" },
        { ClauseType.ValueSetRightLast, ")" },
        { ClauseType.ValueSetOneLeft, "(" },
        { ClauseType.ValueSetOneRight, ")" },
        { ClauseType.ValueSetSeparator, ", " },
        { ClauseType.Where, "WHERE" },
        { ClauseType.In, "IN" },
        { ClauseType.Or, "OR" },
        { ClauseType.And, "AND" },
        { ClauseType.Set, "SET" },
        { ClauseType.RangeLeft, "(" },
        { ClauseType.RangeRight, ")" }
    };

    public SqliteQueryBuilder() : base(Clauses)
    {
    }
}
