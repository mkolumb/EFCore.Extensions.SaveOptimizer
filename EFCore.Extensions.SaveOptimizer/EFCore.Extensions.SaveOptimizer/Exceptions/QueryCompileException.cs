namespace EFCore.Extensions.SaveOptimizer.Exceptions;

public class QueryCompileException : Exception
{
    public QueryCompileException(string message)
        : base(message)
    {
    }
}
