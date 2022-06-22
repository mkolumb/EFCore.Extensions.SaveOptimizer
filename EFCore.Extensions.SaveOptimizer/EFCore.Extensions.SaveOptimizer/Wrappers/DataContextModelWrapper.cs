using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Wrappers;

public class DataContextModelWrapper<TContext> : IDataContextModelWrapper
    where TContext : DbContext
{
    private readonly ConcurrentDictionary<string, IEntityType?> _entityTypes;

    private readonly ConcurrentDictionary<string, string?> _properties;
    private readonly Func<TContext> _resolver;

    private readonly ConcurrentDictionary<string, string?> _schemaNames;

    private readonly ConcurrentDictionary<string, string?> _tableNames;

    public DataContextModelWrapper(Func<TContext> resolver)
    {
        _resolver = resolver;

        _entityTypes = new ConcurrentDictionary<string, IEntityType?>();

        _tableNames = new ConcurrentDictionary<string, string?>();

        _schemaNames = new ConcurrentDictionary<string, string?>();

        _properties = new ConcurrentDictionary<string, string?>();
    }

    public string GetTableName<TEntity>()
    {
        var key = typeof(TEntity).FullName ?? throw new ArgumentNullException(nameof(TEntity));

        return _tableNames.GetOrAdd(key, x => GetEntityType(x)?.GetTableName()) ??
               throw new ArgumentException("Table name is null");
    }

    public string? GetSchema<TEntity>()
    {
        var key = typeof(TEntity).FullName ?? throw new ArgumentNullException(nameof(TEntity));

        return _schemaNames.GetOrAdd(key, x => GetEntityType(x)?.GetSchema());
    }

    public string GetColumn<TEntity>(string propertyName)
    {
        var propertyKey = $"{typeof(TEntity).FullName}|{propertyName}";

        return _properties.GetOrAdd(propertyKey, key =>
        {
            var split = propertyKey.Split("|");

            var entityTypeName = split[0];

            var property = split[1];

            IEntityType? entityType = GetEntityType(entityTypeName);

            IProperty? propertyInfo = entityType?.FindProperty(property);

            StoreObjectIdentifier identifier =
                StoreObjectIdentifier.Table(entityType?.GetTableName()!, entityType?.GetSchema()!);

            return propertyInfo?.GetColumnName(identifier);
        }) ?? throw new ArgumentException("Property is null");
    }

    private IEntityType? GetEntityType(string typeName) =>
        _entityTypes.GetOrAdd(typeName, key =>
        {
            IModel model = _resolver().Model;

            return model.FindEntityType(key ?? throw new ArgumentNullException(nameof(key)));
        });
}
