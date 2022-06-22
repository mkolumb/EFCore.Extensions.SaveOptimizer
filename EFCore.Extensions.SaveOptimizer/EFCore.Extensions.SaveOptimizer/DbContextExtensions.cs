using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Resolvers;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer;

public static class DbContextExtensions
{
    private static readonly QueryCompilerService CompilerService;

    private static readonly QueryTranslatorService TranslatorService;

    static DbContextExtensions()
    {
        CompilerService = new QueryCompilerService(new CompilerWrapperResolver());

        TranslatorService = new QueryTranslatorService();
    }

    public static int SaveChangesOptimized(this DbContext context) => context.SaveChangesOptimized(true);

    public static int SaveChangesOptimized(this DbContext context, bool acceptAllChangesOnSuccess)
    {
        IEnumerable<SqlResult> queries = context.Prepare();

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (transaction == null)
        {
            transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);

            autoCommit = true;
        }

        try
        {
            var rows = 0;

            foreach (SqlResult sql in queries)
            {
                rows += context.Database.ExecuteSqlRaw(sql.Sql, sql.Bindings);
            }

            if (autoCommit)
            {
                transaction.Commit();
            }

            if (acceptAllChangesOnSuccess)
            {
                context.ChangeTracker.AcceptAllChanges();
            }

            return rows;
        }
        catch
        {
            if (autoCommit)
            {
                transaction.Rollback();
            }

            throw;
        }
    }

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context,
        CancellationToken cancellationToken = default) =>
        await context.SaveChangesOptimizedAsync(true, cancellationToken);

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context, bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<SqlResult> queries = context.Prepare();

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (transaction == null)
        {
            transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            autoCommit = true;
        }

        try
        {
            var rows = 0;

            foreach (SqlResult sql in queries)
            {
                rows += await context.Database.ExecuteSqlRawAsync(sql.Sql, sql.Bindings, cancellationToken);
            }

            if (autoCommit)
            {
                await transaction.CommitAsync(cancellationToken);
            }

            if (acceptAllChangesOnSuccess)
            {
                context.ChangeTracker.AcceptAllChanges();
            }

            return rows;
        }
        catch
        {
            if (autoCommit)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            throw;
        }
    }

    private static IEnumerable<SqlResult> Prepare(this DbContext context)
    {
        DataContextModelWrapper wrapper = new(() => context);

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

            QueryDataModel? translation = TranslatorService.Translate(wrapper, entry);

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

        IDictionary<Type, int> executeOrder = context.Model.GetEntityTypes().ResolveEntityHierarchy();

        List<SqlResult> results = new();

        var providerName = context.Database.ProviderName ??
                           throw new ArgumentNullException(nameof(context.Database.ProviderName));

        foreach ((Type type, _) in executeOrder.OrderByDescending(x => x.Value))
        {
            results.AddRange(GetQuery(translations, EntityState.Deleted, type, providerName));
        }

        foreach ((Type type, _) in executeOrder.OrderBy(x => x.Value))
        {
            results.AddRange(GetQuery(translations, EntityState.Added, type, providerName));
        }

        foreach ((Type type, _) in executeOrder.OrderBy(x => x.Value))
        {
            results.AddRange(GetQuery(translations, EntityState.Modified, type, providerName));
        }

        return results;
    }

    private static IEnumerable<SqlResult> GetQuery(
        IReadOnlyDictionary<EntityState, Dictionary<Type, List<QueryDataModel>>> group,
        EntityState state, Type type, string providerName)
    {
        Dictionary<Type, List<QueryDataModel>> queries = group[state];

        if (!queries.ContainsKey(type))
        {
            return Array.Empty<SqlResult>();
        }

        List<QueryDataModel> data = queries[type];

        return CompilerService.Compile(data, providerName);
    }
}
