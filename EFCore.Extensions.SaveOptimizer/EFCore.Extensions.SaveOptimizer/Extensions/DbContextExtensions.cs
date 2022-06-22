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

        QueryDataModel?[] translation = entries
            .Select(entry => _translatorService.Translate(wrapper, entry))
            .Where(x => x != null)
            .ToArray();

        IDictionary<Type, int> executeOrder = context.Model.GetEntityTypes().ResolveEntityHierarchy();

        List<SqlResult> results = new();

        foreach ((Type type, _) in executeOrder.OrderByDescending(x => x.Value))
        {
            QueryDataModel?[] data = translation
                .Where(t => t != null && t.EntityType == type && t.EntityState == EntityState.Deleted).ToArray();

            IEnumerable<SqlResult> compilation = _compilerService.Compile(data, context.Database.ProviderName);

            results.AddRange(compilation);
        }

        foreach ((Type type, _) in executeOrder.OrderBy(x => x.Value))
        {
            QueryDataModel?[] data = translation
                .Where(t => t != null && t.EntityType == type && t.EntityState == EntityState.Added).ToArray();

            IEnumerable<SqlResult> compilation = _compilerService.Compile(data, context.Database.ProviderName);

            results.AddRange(compilation);
        }

        foreach ((Type type, _) in executeOrder.OrderBy(x => x.Value))
        {
            QueryDataModel?[] data = translation
                .Where(t => t != null && t.EntityType == type && t.EntityState == EntityState.Modified).ToArray();

            IEnumerable<SqlResult> compilation = _compilerService.Compile(data, context.Database.ProviderName);

            results.AddRange(compilation);
        }

        return results;
    }
}
