using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using EFCore.Extensions.SaveOptimizer.Internal.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryPreparerService : IQueryPreparerService
{
    private readonly IQueryCompilerService _compilerService;

    private readonly ConcurrentDictionary<string, IDictionary<Type, int>> _orders;

    private readonly IQueryTranslatorService _translatorService;

    private readonly ConcurrentDictionary<string, DataContextModelWrapper> _wrappers;

    public QueryPreparerService(IQueryCompilerService compilerService, IQueryTranslatorService translatorService)
    {
        _compilerService = compilerService;
        _translatorService = translatorService;
        _wrappers = new ConcurrentDictionary<string, DataContextModelWrapper>();
        _orders = new ConcurrentDictionary<string, IDictionary<Type, int>>();
    }

    public void Init(DbContext context)
    {
        var name = GetKey(context);

        if (!_wrappers.ContainsKey(name))
        {
            DataContextModelWrapper wrapper = new(() => context);

            _wrappers[name] = wrapper;
        }

        if (!_orders.ContainsKey(name))
        {
            IDictionary<Type, int> executeOrder = context.Model.ResolveEntityHierarchy();

            _orders[name] = executeOrder;
        }
    }

    public IEnumerable<SqlCommandModel> Prepare(DbContext context, int batchSize = InternalConstants.DefaultBatchSize)
    {
        var name = GetKey(context);

        IEnumerable<EntityEntry> entries = context.ChangeTracker.Entries();

        Dictionary<EntityState, Dictionary<Type, List<QueryDataModel>>> translations = new()
        {
            { EntityState.Added, new Dictionary<Type, List<QueryDataModel>>() },
            { EntityState.Modified, new Dictionary<Type, List<QueryDataModel>>() },
            { EntityState.Deleted, new Dictionary<Type, List<QueryDataModel>>() }
        };

        Dictionary<EntityState, Dictionary<Type, int>> maxParameters = new()
        {
            { EntityState.Added, new Dictionary<Type, int>() },
            { EntityState.Modified, new Dictionary<Type, int>() },
            { EntityState.Deleted, new Dictionary<Type, int>() }
        };

        foreach (EntityEntry entry in entries)
        {
            if (entry.State is not (EntityState.Modified or EntityState.Added or EntityState.Deleted))
            {
                continue;
            }

            QueryDataModel? translation = _translatorService.Translate(_wrappers[name], entry);

            if (translation == null)
            {
                continue;
            }

            Dictionary<Type, int> paramsDictionary = maxParameters[entry.State];

            if (!paramsDictionary.ContainsKey(translation.EntityType))
            {
                paramsDictionary.Add(translation.EntityType, 1);
            }

            Dictionary<Type, List<QueryDataModel>> dictionary = translations[entry.State];

            if (!dictionary.ContainsKey(translation.EntityType))
            {
                dictionary.Add(translation.EntityType, new List<QueryDataModel>());
            }

            dictionary[translation.EntityType].Add(translation);

            if (paramsDictionary[translation.EntityType] < translation.PropertiesCount)
            {
                paramsDictionary[translation.EntityType] = translation.PropertiesCount;
            }
        }

        List<SqlCommandModel> results = new();

        var providerName = context.Database.ProviderName ?? throw new ArgumentException("Provider not known");

        foreach ((Type type, _) in _orders[name].OrderByDescending(x => x.Value))
        {
            foreach (IEnumerable<SqlCommandModel> sqlResults in GetQuery(translations, maxParameters, EntityState.Deleted, type, providerName, batchSize))
            {
                results.AddRange(sqlResults);
            }
        }

        foreach ((Type type, _) in _orders[name].OrderBy(x => x.Value))
        {
            foreach (IEnumerable<SqlCommandModel> sqlResults in GetQuery(translations, maxParameters, EntityState.Added, type, providerName, batchSize))
            {
                results.AddRange(sqlResults);
            }

            foreach (IEnumerable<SqlCommandModel> sqlResults in GetQuery(translations, maxParameters, EntityState.Modified, type, providerName, batchSize))
            {
                results.AddRange(sqlResults);
            }
        }

        return results;
    }

    private static string GetKey(DbContext context)
    {
        var typeName = context.GetType().FullName ?? throw new ArgumentNullException(nameof(context));

        var providerName = context.Database.ProviderName;

        return $"{typeName}_{providerName}";
    }

    private IEnumerable<IEnumerable<SqlCommandModel>> GetQuery(
        IReadOnlyDictionary<EntityState, Dictionary<Type, List<QueryDataModel>>> group,
        IReadOnlyDictionary<EntityState, Dictionary<Type, int>> maxParameters,
        EntityState state,
        Type type,
        string providerName,
        int batchSize)
    {
        Dictionary<Type, List<QueryDataModel>> queries = group[state];

        if (!queries.ContainsKey(type))
        {
            yield break;
        }

        List<QueryDataModel> typedQueries = queries[type];

        if (typedQueries.Count == 0)
        {
            yield break;
        }

        var limit = _compilerService.GetParametersLimit(providerName);

        var maxBatch = limit / maxParameters[state][type];

        batchSize = Math.Min(batchSize, maxBatch);

        List<List<QueryDataModel>> data = new();

        for (var i = 0; i < typedQueries.Count; i++)
        {
            QueryDataModel query = typedQueries[i];

            if (i % batchSize == 0)
            {
                data.Add(new List<QueryDataModel>());
            }

            data.Last().Add(query);
        }

        foreach (List<QueryDataModel> q in data)
        {
            yield return _compilerService.Compile(q, providerName);
        }
    }
}
