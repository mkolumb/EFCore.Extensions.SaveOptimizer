namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public record SqlCommandModel(string? Sql, IReadOnlyCollection<SqlParamModel>? Parameters) : ISqlCommandModel
{
    public IDictionary<string, object?>? NamedBindings { get; } =
        Parameters?.ToDictionary(x => x.Key, x => x.SqlValueModel.Value);
}
