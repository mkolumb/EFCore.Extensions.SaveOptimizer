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

namespace EFCore.Extensions.SaveOptimizer.Extensions;

public static class DbContextExtensions
{
    private static readonly QueryCompilerService _compilerService;

    private static readonly QueryTranslatorService _translatorService;

    static DbContextExtensions()
    {
        _compilerService = new QueryCompilerService(new CompilerWrapperResolver());

        _translatorService = new QueryTranslatorService();
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
        IEnumerable<EntityEntry> entries = context.ChangeTracker.Entries();

        DataContextModelWrapper wrapper = new(() => context);

        QueryDataModel[] translation = entries
            .Select(entry => _translatorService.Translate(wrapper, entry))
            .Where(x => x != null)
            .ToArray()!;

        Dictionary<EntityState, Dictionary<Type, QueryDataModel[]>> group = translation
            .GroupBy(x => x.EntityState)
            .ToDictionary(x => x.Key,
                x => x.GroupBy(i => i.EntityType)
                    .ToDictionary(i => i.Key, i => i.ToArray()));

        IDictionary<Type, int> executeOrder = context.Model.GetEntityTypes().ResolveEntityHierarchy();

        List<SqlResult> results = new();

        var providerName = context.Database.ProviderName ??
                           throw new ArgumentNullException(nameof(context.Database.ProviderName));

        foreach ((Type type, _) in executeOrder.OrderByDescending(x => x.Value))
        {
            if (!group.ContainsKey(EntityState.Deleted))
            {
                break;
            }

            results.AddRange(GetQuery(group, EntityState.Deleted, type, providerName));
        }

        foreach ((Type type, _) in executeOrder.OrderBy(x => x.Value))
        {
            if (!group.ContainsKey(EntityState.Added))
            {
                break;
            }

            results.AddRange(GetQuery(group, EntityState.Added, type, providerName));
        }

        foreach ((Type type, _) in executeOrder.OrderBy(x => x.Value))
        {
            if (!group.ContainsKey(EntityState.Modified))
            {
                break;
            }

            results.AddRange(GetQuery(group, EntityState.Modified, type, providerName));
        }

        return results;
    }

    private static IEnumerable<SqlResult> GetQuery(IReadOnlyDictionary<EntityState, Dictionary<Type, QueryDataModel[]>> group,
        EntityState state, Type type, string providerName)
    {
        Dictionary<Type, QueryDataModel[]> queries = group[state];

        if (!queries.ContainsKey(type))
        {
            return Array.Empty<SqlResult>();
        }

        QueryDataModel[] data = queries[type];

        return _compilerService.Compile(data, providerName);
    }
}
