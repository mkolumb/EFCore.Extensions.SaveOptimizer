﻿using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;

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
        { ClauseType.QueryEnding, "" }
    };

    public OracleQueryBuilder(QueryBuilderConfiguration? configuration) : base(Clauses, configuration)
    {
    }
}
