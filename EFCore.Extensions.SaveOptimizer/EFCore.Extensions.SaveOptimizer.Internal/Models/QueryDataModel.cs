using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class QueryDataModel
{
    public Dictionary<string, object?> Data { get; }

    public string? SchemaName { get; }

    public string TableName { get; }

    public Type EntityType { get; }

    public EntityState EntityState { get; }

    public HashSet<string> PrimaryKeyNames { get; }

    public Dictionary<string, object?>? ConcurrencyTokens { get; }

    public QueryDataModel(Type entityType,
        EntityState entityState,
        string? schemaName,
        string tableName,
        Dictionary<string, object?> data,
        HashSet<string> primaryKeyNames,
        Dictionary<string, object?> concurrencyTokens)
    {
        EntityType = entityType;
        EntityState = entityState;
        SchemaName = schemaName;
        TableName = tableName;
        Data = data;
        PrimaryKeyNames = primaryKeyNames;
        ConcurrencyTokens = concurrencyTokens;
    }
}
