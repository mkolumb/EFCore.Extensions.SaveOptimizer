namespace EFCore.Extensions.SaveOptimizer.Internal.Exceptions;

public class QueryTranslateException : Exception
{
    public QueryTranslateException(string message)
        : base(message)
    {
    }

    public QueryTranslateException(string memberName, params object?[] values)
        : base($"Translator produced different values for same property, member: {memberName}, values: {string.Join(" | ", values)}")
    {
    }
}