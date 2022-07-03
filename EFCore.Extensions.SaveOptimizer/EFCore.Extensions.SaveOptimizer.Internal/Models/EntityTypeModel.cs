using System.Collections.Concurrent;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class EntityTypeModel
{
    public string? Schema { get; set; }

    public string? Table { get; set; }

    public ConcurrentDictionary<string, PropertyTypeModel> Properties { get; set; } = new();
}
