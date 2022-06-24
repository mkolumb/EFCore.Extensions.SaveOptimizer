using EFCore.Extensions.SaveOptimizer.Internal.Models;
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

        Dictionary<string, object?> data = new();

        HashSet<string> primaryKeyNames = new();

        Dictionary<string, object?> tokens = new();

        var propertiesCount = 0;

        foreach (PropertyEntry property in properties)
        {
            var columnName = model.GetColumn(entityType, property.Metadata.Name);

            if (property.Metadata.IsConcurrencyToken)
            {
                tokens.Add(columnName, property.CurrentValue);

                propertiesCount++;
            }

            if (property.Metadata.IsPrimaryKey())
            {
                primaryKeyNames.Add(columnName);

                data.Add(columnName, property.CurrentValue);

                propertiesCount++;

                continue;
            }

            if (entry.State == EntityState.Modified && !property.IsModified)
            {
                continue;
            }

            if (property.Metadata.ValueGenerated != ValueGenerated.Never)
            {
                continue;
            }

            data.Add(columnName, property.CurrentValue);

            if (!property.Metadata.IsConcurrencyToken)
            {
                propertiesCount++;
            }
        }

        return new QueryDataModel(entityType, entry.State, schemaName, tableName, data, primaryKeyNames, tokens, propertiesCount);
    }
}
