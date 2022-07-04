using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;

public static class MapExtensions
{
    public static Dictionary<string, SqlValueModel?> Map(this Dictionary<string, object?> values) =>
        values.ToDictionary(x => x.Key,
            x => new SqlValueModel(x.Value, new PropertyTypeModel(null!, null, null!, null!)))!;

    public static Dictionary<string, object?> Map(this Dictionary<string, SqlValueModel?> values) =>
        values.ToDictionary(x => x.Key, x => x.Value?.Value);
}
