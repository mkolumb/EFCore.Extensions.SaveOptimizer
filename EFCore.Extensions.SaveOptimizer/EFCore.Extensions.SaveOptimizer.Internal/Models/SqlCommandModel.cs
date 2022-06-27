namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlCommandModel : ISqlCommandModel
{
    public string? Sql { get; set; }

    public IDictionary<string, object?>? NamedBindings =>
        Parameters?.ToDictionary(x => x.Key, x => x.SqlValueModel.Value);

    public ICollection<SqlParamModel>? Parameters { get; set; }
}
