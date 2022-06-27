namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public interface ISqlCommandModel
{
    string? Sql { get; }

    IDictionary<string, object?>? NamedBindings { get; }

    public IList<SqlParamModel>? Parameters { get; }
}
