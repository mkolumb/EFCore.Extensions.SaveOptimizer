using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Wrappers;

public class DataContextModelWrapper : IDataContextModelWrapper
{
    private readonly ConcurrentDictionary<string, IEntityType?> _entityTypes;

    private readonly ConcurrentDictionary<string, string?> _properties;
    private readonly Func<DbContext> _resolver;

    private readonly ConcurrentDictionary<string, string?> _schemaNames;

    private readonly ConcurrentDictionary<string, string?> _tableNames;

    public DataContextModelWrapper(Func<DbContext> resolver)
    {
        _resolver = resolver;

        _entityTypes = new ConcurrentDictionary<string, IEntityType?>();

        _tableNames = new ConcurrentDictionary<string, string?>();

        _schemaNames = new ConcurrentDictionary<string, string?>();

        _properties = new ConcurrentDictionary<string, string?>();
    }

    public string GetTableName(Type entityType)
    {
        var key = entityType.FullName ?? throw new ArgumentNullException(nameof(entityType));

        return _tableNames.GetOrAdd(key, x => GetEntityType(x)?.GetTableName()) ??
               throw new ArgumentException("Table name is null");
    }

    public string? GetSchema(Type entityType)
    {
        var key = entityType.FullName ?? throw new ArgumentNullException(nameof(entityType));

        return _schemaNames.GetOrAdd(key, x => GetEntityType(x)?.GetSchema());
    }

    public string GetColumn(Type entityType, string propertyName)
    {
        var propertyKey = $"{entityType.FullName}|{propertyName}";

        return _properties.GetOrAdd(propertyKey, _ =>
        {
            var split = propertyKey.Split("|");

            var entityTypeName = split[0];

            var property = split[1];

            IEntityType? type = GetEntityType(entityTypeName);

            IProperty? propertyInfo = type?.FindProperty(property);

            StoreObjectIdentifier identifier = StoreObjectIdentifier.Table(type?.GetTableName()!, type?.GetSchema()!);

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
