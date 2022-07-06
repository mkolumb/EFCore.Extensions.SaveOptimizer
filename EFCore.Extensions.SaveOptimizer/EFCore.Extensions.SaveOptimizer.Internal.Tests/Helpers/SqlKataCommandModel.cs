using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;

public class SqlKataCommandModel : ISqlCommandModel
{
    public string? Sql { get; set; }
    public IDictionary<string, object?>? NamedBindings { get; set; }
    public IReadOnlyCollection<SqlParamModel>? Parameters { get; set; }
    public int? ExpectedRows { get; set; }
}
