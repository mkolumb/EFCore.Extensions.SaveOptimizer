namespace EFCore.Extensions.SaveOptimizer.Internal.Exceptions;

public class QueryCompileException : Exception
{
    public QueryCompileException(string message)
        : base(message)
    {
    }
}
