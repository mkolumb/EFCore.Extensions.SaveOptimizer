using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class QueryDataModel
{
    public Dictionary<string, SqlValueModel?> Data { get; }

    public string? SchemaName { get; }

    public string TableName { get; }

    public Type EntityType { get; }

    public EntityState EntityState { get; }

    public HashSet<string> PrimaryKeyNames { get; }

    public Dictionary<string, SqlValueModel?>? ConcurrencyTokens { get; }

    public int PropertiesCount { get; }

    public QueryDataModel(Type entityType,
        EntityState entityState,
        string? schemaName,
        string tableName,
        Dictionary<string, SqlValueModel?> data,
        HashSet<string> primaryKeyNames,
        Dictionary<string, SqlValueModel?> concurrencyTokens,
        int propertiesCount)
    {
        EntityType = entityType;
        EntityState = entityState;
        SchemaName = schemaName;
        TableName = tableName;
        Data = data;
        PrimaryKeyNames = primaryKeyNames;
        ConcurrencyTokens = concurrencyTokens;
        PropertiesCount = propertiesCount;
    }
}
