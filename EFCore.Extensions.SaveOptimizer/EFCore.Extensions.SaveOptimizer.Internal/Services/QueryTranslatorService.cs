﻿using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryTranslatorService : IQueryTranslatorService
{
    public QueryDataModel? Translate(IDataContextModelWrapper model, EntityEntry entry)
    {
        if (entry.State is EntityState.Detached or EntityState.Unchanged)
        {
            return null;
        }

        Type entityType = entry.Entity.GetType();

        PropertyEntry[] properties = entry.Properties.ToArray();

        var tableName = model.GetTableName(entityType);
        var schemaName = model.GetSchema(entityType);

        Dictionary<string, SqlValueModel?> data = new();

        HashSet<string> primaryKeyNames = new();

        Dictionary<string, SqlValueModel?> tokens = new();

        var propertiesCount = 0;

        foreach (PropertyEntry property in properties)
        {
            PropertyTypeModel column = model.GetColumn(entityType, property.Metadata.Name);

            if (property.Metadata.IsConcurrencyToken)
            {
                tokens.Add(column.ColumnName, GetSqlValueModel(property, column));

                propertiesCount++;
            }

            if (property.Metadata.IsPrimaryKey())
            {
                primaryKeyNames.Add(column.ColumnName);

                if (ShouldStoreValue(property, entry))
                {
                    data.Add(column.ColumnName, GetSqlValueModel(property, column));
                }

                propertiesCount++;

                continue;
            }

            if (!ShouldStoreValue(property, entry))
            {
                continue;
            }

            data.Add(column.ColumnName, GetSqlValueModel(property, column));

            if (!property.Metadata.IsConcurrencyToken)
            {
                propertiesCount++;
            }
        }

        return new QueryDataModel(entityType, entry.State, schemaName, tableName, data, primaryKeyNames, tokens,
            propertiesCount);
    }

    private static bool ShouldStoreValue(PropertyEntry property, EntityEntry entry)
    {
        if (property.Metadata.IsPrimaryKey() && entry.State != EntityState.Added)
        {
            return true;
        }

        if (entry.State == EntityState.Modified && !property.IsModified)
        {
            return false;
        }

        if (property.Metadata.ValueGenerated == ValueGenerated.Never)
        {
            return true;
        }

        if (property.Metadata.GetBeforeSaveBehavior() != PropertySaveBehavior.Save)
        {
            return false;
        }

        return !property.IsTemporary;
    }

    private static SqlValueModel GetSqlValueModel(MemberEntry property, PropertyTypeModel column)
    {
        var value = property.CurrentValue;

        return new SqlValueModel(value, column);
    }
}
