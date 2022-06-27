﻿namespace EFCore.Extensions.SaveOptimizer.Internal.Enums;

public enum ClauseType
{
    Insert,
    Update,
    Delete,
    Values,
    ValuesOne,
    ValueEscapeLeft,
    ValueEscapeRight,
    TableEscape,
    ParameterPrefix,
    ValueSetLeft,
    ValueSetRight,
    ValueSetRightLast,
    ValueSetOneLeft,
    ValueSetOneRight,
    ValueSetSeparator,
    Where,
    In,
    Or,
    And,
    Set,
    RangeLeft,
    RangeRight,
    QueryEnding
}
