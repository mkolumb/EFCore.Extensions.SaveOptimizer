using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Wrappers;

public class DataContextModelWrapper<TContext> : IDataContextModelWrapper
    where TContext : DbContext
{
    private readonly ConcurrentDictionary<string, IEntityType> _entityTypes;

    private readonly ConcurrentDictionary<string, string> _properties;
    private readonly Func<TContext> _resolver;

    private readonly ConcurrentDictionary<string, string> _schemaNames;

    private readonly ConcurrentDictionary<string, string> _tableNames;

    public DataContextModelWrapper(Func<TContext> resolver)
    {
        _resolver = resolver;

        _entityTypes = new ConcurrentDictionary<string, IEntityType>();

        _tableNames = new ConcurrentDictionary<string, string>();

        _schemaNames = new ConcurrentDictionary<string, string>();

        _properties = new ConcurrentDictionary<string, string>();
    }

    public string GetTableName<TEntity>() =>
        _tableNames.GetOrAdd(typeof(TEntity).FullName, key => GetEntityType(key).GetTableName());

    public string GetSchema<TEntity>() =>
        _schemaNames.GetOrAdd(typeof(TEntity).FullName, key => GetEntityType(key).GetSchema());

    public string GetColumn<TEntity>(string propertyName)
    {
        var propertyKey = $"{typeof(TEntity).FullName}|{propertyName}";

        return _properties.GetOrAdd(propertyKey, key =>
        {
            var split = propertyKey.Split("|");

            var entityTypeName = split[0];

            var property = split[1];

            return GetEntityType(entityTypeName).FindProperty(property).GetColumnName();
        });
    }

    private IEntityType GetEntityType(string typeName) =>
        _entityTypes.GetOrAdd(typeName, key =>
        {
            IModel model = _resolver().Model;

            return model.FindEntityType(key ?? throw new ArgumentNullException(nameof(key)));
        });
}
