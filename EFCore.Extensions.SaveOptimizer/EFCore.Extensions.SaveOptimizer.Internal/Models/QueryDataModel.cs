using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public record QueryDataModel(Type EntityType,
    EntityState EntityState,
    string? SchemaName,
    string TableName,
    Dictionary<string, SqlValueModel?> Data,
    HashSet<string> PrimaryKeyNames,
    Dictionary<string, SqlValueModel?>? ConcurrencyTokens,
    int PropertiesCount);
