using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Wrappers;

public class DataContextModelWrapper : IDataContextModelWrapper
{
    private readonly ConcurrentDictionary<string, EntityTypeModel?> _entityTypes;

    public DataContextModelWrapper(Func<DbContext> resolver)
    {
        _entityTypes = new ConcurrentDictionary<string, EntityTypeModel?>();

        Init(resolver);
    }

    public string GetTableName(Type entityType)
    {
        var key = entityType.FullName ?? throw new ArgumentNullException(nameof(entityType));

        return _entityTypes[key]?.Table ?? throw new ArgumentException("Table name is null");
    }

    public string? GetSchema(Type entityType)
    {
        var key = entityType.FullName ?? throw new ArgumentNullException(nameof(entityType));

        return _entityTypes[key]?.Schema;
    }

    public string GetColumn(Type entityType, string propertyName)
    {
        var key = entityType.FullName ?? throw new ArgumentNullException(nameof(entityType));

        return _entityTypes[key]?.Properties[propertyName].ColumnName ?? throw new ArgumentException("Property is null");
    }

    private void Init(Func<DbContext> resolver)
    {
        DbContext context = resolver();

        foreach (IEntityType entityType in context.Model.GetEntityTypes())
        {
            var name = entityType.ClrType.FullName ?? throw new ArgumentException("Unable to retrieve full name");

            EntityTypeModel model = new() { Schema = entityType.GetSchema(), Table = entityType.GetTableName() };

            foreach (IProperty property in entityType.GetProperties())
            {
                var key = property.Name;

                StoreObjectIdentifier identifier = StoreObjectIdentifier.Table(model.Table!, model.Schema!);

                model.Properties[key] = new PropertyTypeModel { ColumnName = property.GetColumnName(identifier) };
            }

            _entityTypes[name] = model;
        }
    }
}
