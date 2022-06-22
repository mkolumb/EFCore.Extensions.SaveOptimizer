﻿using System.Collections.Concurrent;
using System.Collections.Immutable;
using EFCore.Extensions.SaveOptimizer.Internal.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryPreparerService : IQueryPreparerService
{
    private const int BatchSize = 1000;

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

    public IEnumerable<SqlResult> Prepare(DbContext context)
    {
        var name = GetKey(context);

        IEnumerable<EntityEntry> entries = context.ChangeTracker.Entries();

        Dictionary<EntityState, Dictionary<Type, List<QueryDataModel>>> translations =
            new()
            {
                { EntityState.Added, new Dictionary<Type, List<QueryDataModel>>() },
                { EntityState.Modified, new Dictionary<Type, List<QueryDataModel>>() },
                { EntityState.Deleted, new Dictionary<Type, List<QueryDataModel>>() }
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

            Dictionary<Type, List<QueryDataModel>> dictionary = translations[entry.State];

            if (!dictionary.ContainsKey(translation.EntityType))
            {
                dictionary.Add(translation.EntityType, new List<QueryDataModel>());
            }

            dictionary[translation.EntityType].Add(translation);
        }

        List<SqlResult> results = new();

        var providerName = context.Database.ProviderName ??
                           throw new ArgumentNullException(nameof(context.Database.ProviderName));

        foreach ((Type type, _) in _orders[name].OrderByDescending(x => x.Value))
        {
            results.AddRange(GetQuery(translations, EntityState.Deleted, type, providerName).SelectMany(x => x));
        }

        foreach ((Type type, _) in _orders[name].OrderBy(x => x.Value))
        {
            results.AddRange(GetQuery(translations, EntityState.Added, type, providerName).SelectMany(x => x));
        }

        foreach ((Type type, _) in _orders[name].OrderBy(x => x.Value))
        {
            results.AddRange(GetQuery(translations, EntityState.Modified, type, providerName).SelectMany(x => x));
        }

        return results;
    }

    private static string GetKey(DbContext context)
    {
        var typeName = context.GetType().FullName ?? throw new ArgumentNullException(nameof(context));

        var providerName = context.Database.ProviderName;

        return $"{typeName}_{providerName}";
    }

    private IEnumerable<IEnumerable<SqlResult>> GetQuery(
        IReadOnlyDictionary<EntityState, Dictionary<Type, List<QueryDataModel>>> group,
        EntityState state, Type type, string providerName)
    {
        Dictionary<Type, List<QueryDataModel>> queries = group[state];

        if (!queries.ContainsKey(type))
        {
            yield break;
        }

        ImmutableArray<ImmutableArray<QueryDataModel>> data = queries[type].ToChunks(BatchSize);

        foreach (ImmutableArray<QueryDataModel> q in data)
        {
            yield return _compilerService.Compile(q, providerName);
        }
    }
}