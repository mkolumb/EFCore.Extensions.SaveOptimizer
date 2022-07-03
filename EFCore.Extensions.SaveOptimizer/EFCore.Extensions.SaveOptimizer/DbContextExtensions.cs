using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Factories;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer;

public static class DbContextExtensions
{
    private static readonly IQueryPreparerService QueryPreparerService;
    private static readonly IQueryExecutorService QueryExecutorService;

    static DbContextExtensions()
    {
        QueryCompilerService compilerService = new(new QueryBuilderFactory());

        QueryTranslatorService translatorService = new();

        QueryExecutionConfiguratorService configuratorService = new();

        QueryPreparerService = new QueryPreparerService(compilerService, translatorService, configuratorService);

        QueryExecutorService = new QueryExecutorService();
    }

    public static int SaveChangesOptimized(this DbContext context) =>
        context.SaveChangesOptimized(null);

    public static int SaveChangesOptimized(this DbContext context, QueryExecutionConfiguration? configuration)
    {
        QueryPreparerService.Init(context);

        IEnumerable<ISqlCommandModel> queries = QueryPreparerService.Prepare(context, configuration);

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (transaction == null)
        {
            transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);

            autoCommit = true;
        }

        var timeout = context.Database.GetCommandTimeout();

        try
        {
            var rows = 0;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (ISqlCommandModel sql in queries)
            {
                rows += QueryExecutorService.Execute(context, transaction, sql, timeout);
            }

            if (autoCommit)
            {
                transaction.Commit();
            }

            MarkEntitiesAfterSave(context);

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
        finally
        {
            if (autoCommit)
            {
                transaction.Dispose();
            }
        }
    }

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context,
        CancellationToken cancellationToken = default) =>
        await context.SaveChangesOptimizedAsync(null, cancellationToken);

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context,
        QueryExecutionConfiguration? configuration,
        CancellationToken cancellationToken = default)
    {
        QueryPreparerService.Init(context);

        IEnumerable<ISqlCommandModel> queries = QueryPreparerService.Prepare(context, configuration);

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (transaction == null)
        {
            transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken)
                .ConfigureAwait(false);

            autoCommit = true;
        }

        var timeout = context.Database.GetCommandTimeout();

        try
        {
            var rows = 0;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (ISqlCommandModel sql in queries)
            {
                rows += await QueryExecutorService
                    .ExecuteAsync(context, transaction, sql, timeout, cancellationToken)
                    .ConfigureAwait(false);
            }

            if (autoCommit)
            {
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            }

            MarkEntitiesAfterSave(context);

            return rows;
        }
        catch
        {
            if (autoCommit)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            }

            throw;
        }
        finally
        {
            if (autoCommit)
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    private static void MarkEntitiesAfterSave(DbContext context)
    {
        foreach (EntityEntry entityEntry in context.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
        }
    }
}
