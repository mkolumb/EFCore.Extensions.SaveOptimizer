using EFCore.Extensions.SaveOptimizer.Exceptions;
using EFCore.Extensions.SaveOptimizer.Models;
using EFCore.Extensions.SaveOptimizer.Resolvers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Services;

public class QueryTranslatorService : IQueryTranslatorService
{
    private readonly IDataContextModelWrapperResolver _resolver;

    public QueryTranslatorService(IDataContextModelWrapperResolver resolver) => _resolver = resolver;

    public QueryDataModel? Translate<TContext, TEntity>(EntityEntry entry)
        where TContext : DbContext
        where TEntity : class
    {
        if (entry.State is EntityState.Detached or EntityState.Unchanged)
        {
            return null;
        }

        var properties = entry.Properties.ToArray();

        var model = _resolver.Resolve<TContext>();

        var tableName = model.GetTableName<TEntity>();
        var schemaName = model.GetSchema<TEntity>();

        var primaryKeys = properties
            .Where(x => x.Metadata.IsPrimaryKey())
            .ToDictionary(x => model.GetColumn<TEntity>(x.Metadata.Name), x => x.CurrentValue);

        var data = new Dictionary<string, object?>();

        foreach (var (key, value) in primaryKeys)
        {
            data.Add(key, value);
        }

        foreach (var property in properties)
        {
            if (entry.State == EntityState.Modified && !property.IsModified)
            {
                continue;
            }

            switch (property.Metadata.ValueGenerated)
            {
                case ValueGenerated.OnAdd when entry.State == EntityState.Added:
                case ValueGenerated.OnUpdate when entry.State == EntityState.Modified:
                case ValueGenerated.OnUpdateSometimes when entry.State == EntityState.Modified:
                case ValueGenerated.OnAddOrUpdate when entry.State is EntityState.Added or EntityState.Modified:
                    continue;
            }

            var columnName = model.GetColumn<TEntity>(property.Metadata.Name);

            if (primaryKeys.ContainsKey(columnName))
            {
                continue;
            }

            var newValue = property.CurrentValue;

            if (data.ContainsKey(columnName))
            {
                var oldValue = data[columnName];

                if (oldValue != newValue)
                {
                    throw new QueryTranslateException(property.Metadata.Name, oldValue, newValue);
                }
            }

            data[columnName] = newValue;
        }

        var concurrencyTokens = properties.Where(x => x.Metadata.IsConcurrencyToken);

        var tokens = concurrencyTokens
            .ToDictionary(x => model.GetColumn<TEntity>(x.Metadata.Name), x => x.CurrentValue);

        return new QueryDataModel(typeof(TEntity), entry.State, schemaName, tableName, data, primaryKeys.Select(x => x.Key).ToArray(), tokens);
    }
}
