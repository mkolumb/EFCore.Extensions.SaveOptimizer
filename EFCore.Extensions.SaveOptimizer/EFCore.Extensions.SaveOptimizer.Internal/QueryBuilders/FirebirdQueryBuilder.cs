﻿using EFCore.Extensions.SaveOptimizer.Internal.Enums;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public class FirebirdQueryBuilder : BaseQueryBuilder
{
    private static readonly Dictionary<ClauseType, string> Clauses = new()
    {
        { ClauseType.Insert, "INSERT INTO" },
        { ClauseType.Update, "UPDATE FROM" },
        { ClauseType.Delete, "DELETE FROM" },
        { ClauseType.Values, "" },
        { ClauseType.ValuesOne, "VALUES " },
        { ClauseType.ValueEscapeLeft, "\"" },
        { ClauseType.ValueEscapeRight, "\"" },
        { ClauseType.TableEscape, "\".\"" },
        { ClauseType.ParameterPrefix, "@p" },
        { ClauseType.ValueSetLeft, "SELECT " },
        { ClauseType.ValueSetRight, " FROM RDB$DATABASE UNION ALL " },
        { ClauseType.ValueSetRightLast, " FROM RDB$DATABASE" },
        { ClauseType.ValueSetOneLeft, "(" },
        { ClauseType.ValueSetOneRight, ")" },
        { ClauseType.ValueSetSeparator, "" }
    };

    public FirebirdQueryBuilder() : base(Clauses, CaseType.Uppercase)
    {
    }
}