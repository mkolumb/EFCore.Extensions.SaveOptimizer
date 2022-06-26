namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlCommandModel
{
    public string? Sql { get; set; }

    public IDictionary<string, object?>? NamedBindings { get; set; }
}
