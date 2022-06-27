using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;

public class SqlKataCommandModel : ISqlCommandModel
{
    public string? Sql { get; set; }
    public IDictionary<string, object?>? NamedBindings { get; set; }
    public IList<SqlParamModel>? Parameters { get; set; }
}
