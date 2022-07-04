using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

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

    public PropertyTypeModel GetColumn(Type entityType, string propertyName)
    {
        var key = entityType.FullName ?? throw new ArgumentNullException(nameof(entityType));

        return _entityTypes[key]?.Properties[propertyName] ?? throw new ArgumentException("Property is null");
    }

    private void Init(Func<DbContext> resolver)
    {
        DbContext context = resolver();

        var isRelational = context.Database.IsRelational();

        foreach (IEntityType entityType in context.Model.GetEntityTypes())
        {
            var name = entityType.ClrType.FullName ?? throw new ArgumentException("Unable to retrieve full name");

            EntityTypeModel model = new() { Schema = entityType.GetSchema(), Table = entityType.GetTableName() };

            foreach (IProperty property in entityType.GetProperties())
            {
                var key = property.Name;

                StoreObjectIdentifier identifier = StoreObjectIdentifier.Table(model.Table!, model.Schema!);

                model.Properties[key] = GetModel(property, identifier, isRelational);
            }

            _entityTypes[name] = model;
        }
    }

    private static PropertyTypeModel GetModel(IReadOnlyProperty property, StoreObjectIdentifier identifier,
        bool isRelational)
    {
        var name = property.GetColumnName(identifier) ?? throw new ArgumentException("Unable to find column name");
        RelationalTypeMapping? mapping = isRelational ? property.GetRelationalTypeMapping() : null;
        var type = isRelational ? property.GetColumnType() : null;

        Func<IDbCommand, string, object?, DbParameter> resolver;

        if (mapping != null)
        {
            resolver = (dbCmd, key, value) =>
            {
                if (dbCmd is not DbCommand cmd)
                {
                    throw new ArgumentException("Unexpected command type", nameof(dbCmd));
                }

                return mapping.CreateParameter(cmd, key, value, property.IsNullable);
            };
        }
        else
        {
            resolver = (dbCmd, key, value) => CreateDefaultParameter(dbCmd, key, value, property);
        }

        var signature = $"{property.DeclaringType.Name}_{property.ClrType.Name}_{property.IsNullable}_{property.GetScale()}_{property.GetPrecision()}";

        return new PropertyTypeModel(name, type, resolver, signature);
    }

    private static DbParameter CreateDefaultParameter(IDbCommand dbCmd, string paramKey, object? value, IReadOnlyProperty property)
    {
        if (dbCmd is not DbCommand cmd)
        {
            throw new ArgumentException("Unexpected command type", nameof(dbCmd));
        }

        var parameter = cmd.CreateParameter();
        parameter.ParameterName = paramKey;
        parameter.Value = value;
        parameter.DbType = DbType.String;
        parameter.IsNullable = property.IsNullable;
        parameter.Direction = ParameterDirection.Input;

        var precision = property.GetPrecision();

        if (precision.HasValue)
        {
            parameter.Precision = (byte)precision.Value;
        }

        var scale = property.GetScale();

        if (scale.HasValue)
        {
            parameter.Scale = (byte)scale.Value;
        }

        return parameter;
    }
}
