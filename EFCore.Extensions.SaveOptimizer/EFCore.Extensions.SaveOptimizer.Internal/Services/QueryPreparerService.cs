using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
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

    private readonly IQueryTranslatorService _translatorService;

    private readonly IDbContextDependencyResolverService _dbContextDependencyResolverService;

    private readonly ConcurrentDictionary<object, DataContextModelWrapper> _wrappers;

    private readonly ConcurrentDictionary<object, IDictionary<Type, int>> _orders;

    public QueryPreparerService(IQueryCompilerService compilerService,
        IQueryTranslatorService translatorService,
        IDbContextDependencyResolverService dbContextDependencyResolverService)
    {
        _compilerService = compilerService;
        _translatorService = translatorService;
        _dbContextDependencyResolverService = dbContextDependencyResolverService;
        _wrappers = new ConcurrentDictionary<object, DataContextModelWrapper>();
        _orders = new ConcurrentDictionary<object, IDictionary<Type, int>>();
    }

    public void Init(DbContext context)
    {
        var name = GetKey(context);

        if (!_wrappers.ContainsKey(name))
        {
            DataContextModelWrapper wrapper = new(() => context);

            _wrappers[name] = wrapper;
        }

        // ReSharper disable once InvertIf
        if (!_orders.ContainsKey(name))
        {
            IDictionary<Type, int> executeOrder = context.Model.ResolveEntityHierarchy();

            _orders[name] = executeOrder;
        }
    }

    public QueryPreparationModel Prepare(DbContext context, QueryExecutionConfiguration configuration)
    {
        var name = GetKey(context);

        var expectedRows = 0;

        EntityEntry[] entries = context.ChangeTracker.Entries().ToArray();

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

            expectedRows++;

            Dictionary<Type, int> paramsDictionary = maxParameters[entry.State];

            paramsDictionary.TryAdd(translation.EntityType, 1);

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

        List<ISqlCommandModel> results = new();

        foreach ((Type type, _) in _orders[name].OrderByDescending(x => x.Value))
        {
            foreach (IEnumerable<ISqlCommandModel> sqlResults in GetQuery(translations, maxParameters,
                         EntityState.Deleted, type, configuration))
            {
                results.AddRange(sqlResults);
            }
        }

        foreach ((Type type, _) in _orders[name].OrderBy(x => x.Value))
        {
            foreach (IEnumerable<ISqlCommandModel> sqlResults in GetQuery(translations, maxParameters,
                         EntityState.Added, type, configuration))
            {
                results.AddRange(sqlResults);
            }

            foreach (IEnumerable<ISqlCommandModel> sqlResults in GetQuery(translations, maxParameters,
                         EntityState.Modified, type, configuration))
            {
                results.AddRange(sqlResults);
            }
        }

        return new QueryPreparationModel(results, entries, expectedRows);
    }

    private object GetKey(DbContext context)
    {
        var factory = _dbContextDependencyResolverService.GetModelCacheKeyFactory(context);

        return factory.Create(context, false);
    }

    private IEnumerable<IEnumerable<ISqlCommandModel>> GetQuery(
        IReadOnlyDictionary<EntityState, Dictionary<Type, List<QueryDataModel>>> group,
        IReadOnlyDictionary<EntityState, Dictionary<Type, int>> maxParameters,
        EntityState state,
        Type type,
        QueryExecutionConfiguration configuration)
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

        var limit = configuration.ParametersLimit ?? InternalConstants.DefaultParametersLimit;

        var batchSize = GetBatchSize(configuration, state);

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
            yield return _compilerService.Compile(q, configuration.BuilderConfiguration);
        }
    }

    private static int GetBatchSize(QueryExecutionConfiguration configuration, EntityState state) =>
        state switch
        {
            EntityState.Added when configuration.InsertBatchSize.HasValue => configuration.InsertBatchSize.Value,
            EntityState.Modified when configuration.UpdateBatchSize.HasValue => configuration.UpdateBatchSize.Value,
            EntityState.Deleted when configuration.DeleteBatchSize.HasValue => configuration.DeleteBatchSize.Value,
            _ => configuration.BatchSize ?? InternalConstants.DefaultBatchSize
        };
}
