namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlCommandModel : ISqlCommandModel
{
    public SqlCommandModel(string? sql, IReadOnlyCollection<SqlParamModel>? parameters)
    {
        Sql = sql;
        Parameters = parameters;
    }

    public string? Sql { get; }

    public IDictionary<string, object?>? NamedBindings =>
        Parameters?.ToDictionary(x => x.Key, x => x.SqlValueModel.Value);

    public IReadOnlyCollection<SqlParamModel>? Parameters { get; }
}
