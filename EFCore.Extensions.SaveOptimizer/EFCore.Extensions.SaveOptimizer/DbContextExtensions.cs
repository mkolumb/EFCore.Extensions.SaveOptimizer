using System.Data;
using System.Data.Common;
using Dapper;
using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using EFCore.Extensions.SaveOptimizer.Internal.Resolvers;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable UnusedMember.Global

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
        context.SaveChangesOptimized(InternalConstants.DefaultBatchSize);

    public static int SaveChangesOptimized(this DbContext context, int batchSize)
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

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (SqlResult sql in queries)
            {
                rows += Execute(transaction, context.Database.GetCommandTimeout(), sql);
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
        await context.SaveChangesOptimizedAsync(InternalConstants.DefaultBatchSize, cancellationToken);

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context, int batchSize,
        CancellationToken cancellationToken = default)
    {
        QueryPreparerService.Init(context);

        IEnumerable<SqlResult> queries = QueryPreparerService.Prepare(context, batchSize);

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (transaction == null)
        {
            transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken)
                .ConfigureAwait(false);

            autoCommit = true;
        }

        try
        {
            var rows = 0;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (SqlResult sql in queries)
            {
                rows += await ExecuteAsync(transaction, sql, context.Database.GetCommandTimeout(), cancellationToken)
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

    private static int Execute(IDbContextTransaction transaction,
        int? timeout,
        SqlResult sql)
    {
        DbTransaction dbTransaction = transaction.GetDbTransaction();

        CommandDefinition commandDefinition = new(
            sql.Sql,
            sql.NamedBindings,
            dbTransaction,
            timeout);

        return dbTransaction.Connection.Execute(commandDefinition);
    }

    private static async Task<int> ExecuteAsync(IDbContextTransaction transaction,
        SqlResult sql,
        int? timeout,
        CancellationToken cancellationToken)
    {
        DbTransaction dbTransaction = transaction.GetDbTransaction();

        CommandDefinition commandDefinition = new(
            sql.Sql,
            sql.NamedBindings,
            dbTransaction,
            timeout,
            cancellationToken: cancellationToken);

        return await dbTransaction.Connection.ExecuteAsync(commandDefinition);
    }

    private static void MarkEntitiesAfterSave(DbContext context)
    {
        foreach (EntityEntry entityEntry in context.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
        }
    }
}
