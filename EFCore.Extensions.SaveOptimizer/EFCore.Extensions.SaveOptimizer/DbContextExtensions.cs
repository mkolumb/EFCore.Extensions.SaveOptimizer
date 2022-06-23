using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using EFCore.Extensions.SaveOptimizer.Internal.Resolvers;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer;

public static class DbContextExtensions
{
    private static readonly IQueryPreparerService QueryPreparerService;

    static DbContextExtensions()
    {
        QueryCompilerService compilerService = new(new CompilerWrapperResolver());

        QueryTranslatorService translatorService = new();

        QueryPreparerService = new QueryPreparerService(compilerService, translatorService);
    }

    public static int SaveChangesOptimized(this DbContext context) =>
        context.SaveChangesOptimized(true, InternalConstants.DefaultBatchSize);

    public static int SaveChangesOptimized(this DbContext context, bool acceptAllChangesOnSuccess) =>
        context.SaveChangesOptimized(acceptAllChangesOnSuccess, InternalConstants.DefaultBatchSize);

    public static int SaveChangesOptimized(this DbContext context,
        bool acceptAllChangesOnSuccess,
        int batchSize)
    {
        QueryPreparerService.Init(context);

        IEnumerable<SqlResult> queries = QueryPreparerService.Prepare(context, batchSize);

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
        await context.SaveChangesOptimizedAsync(true, InternalConstants.DefaultBatchSize, cancellationToken);

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context, bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default) =>
        await context.SaveChangesOptimizedAsync(acceptAllChangesOnSuccess, InternalConstants.DefaultBatchSize,
            cancellationToken);

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context,
        bool acceptAllChangesOnSuccess,
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        QueryPreparerService.Init(context);

        IEnumerable<SqlResult> queries = QueryPreparerService.Prepare(context, batchSize);

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
}
