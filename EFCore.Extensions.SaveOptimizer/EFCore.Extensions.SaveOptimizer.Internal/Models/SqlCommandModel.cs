namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlCommandModel : ISqlCommandModel
{
    public SqlCommandModel(string? sql, IReadOnlyCollection<SqlParamModel>? parameters, int? expectedRows)
    {
        Sql = sql;
        Parameters = parameters;
        ExpectedRows = expectedRows;
    }

    public string? Sql { get; }

    public IDictionary<string, object?>? NamedBindings =>
        Parameters?.ToDictionary(x => x.Key, x => x.SqlValueModel.Value);

    public IReadOnlyCollection<SqlParamModel>? Parameters { get; }

    public int? ExpectedRows { get; }
}
