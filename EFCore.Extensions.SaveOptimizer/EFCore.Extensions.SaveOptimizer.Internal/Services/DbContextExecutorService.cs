﻿using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class DbContextExecutorService : IDbContextExecutorService
{
    private readonly IDbContextDependencyResolverService _dbContextDependencyResolverService;
    private readonly IQueryExecutionConfiguratorService _queryExecutionConfiguratorService;
    private readonly IQueryExecutorService _queryExecutorService;
    private readonly IQueryPreparerService _queryPreparerService;

    public DbContextExecutorService(IQueryPreparerService queryPreparerService,
        IQueryExecutorService queryExecutorService,
        IQueryExecutionConfiguratorService queryExecutionConfiguratorService,
        IDbContextDependencyResolverService dbContextDependencyResolverService)
    {
        _queryPreparerService = queryPreparerService;
        _queryExecutorService = queryExecutorService;
        _queryExecutionConfiguratorService = queryExecutionConfiguratorService;
        _dbContextDependencyResolverService = dbContextDependencyResolverService;
    }

    public int SaveChangesOptimized(DbContext context, QueryExecutionConfiguration? configuration)
    {
        ILogger logger = _dbContextDependencyResolverService.GetLogger(context);

        configuration = Init(context, configuration);

        QueryPreparationModel queries = _queryPreparerService.Prepare(context, configuration);

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (configuration.AutoTransactionEnabled == true)
        {
            if (transaction == null)
            {
                IsolationLevel? isolationLevel = configuration.AutoTransactionIsolationLevel;

                if (!isolationLevel.HasValue)
                {
                    throw new ArgumentException("Auto transaction isolation level should be set");
                }

                transaction = context.Database.BeginTransaction(isolationLevel.Value);

                autoCommit = true;
            }
        }

        if (transaction == null)
        {
            throw new ArgumentException(
                "There is no transaction on DbContext, enable auto transaction or attach transaction to DbContext");
        }

        var timeout = context.Database.GetCommandTimeout();

        try
        {
            var rows = 0;

            ConcurrencyTokenBehavior? behavior = configuration.ConcurrencyTokenBehavior;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (ISqlCommandModel sql in queries.Queries)
            {
                var affected = _queryExecutorService.Execute(context, configuration, transaction, sql, timeout);

                rows += affected;

                if (affected >= 0 || behavior == ConcurrencyTokenBehavior.SaveWhatPossible)
                {
                    continue;
                }

                behavior = ConcurrencyTokenBehavior.SaveWhatPossible;

                logger.LogWarning(
                    "Returned {affected} affected rows, switch behavior to {behavior} for the current execution",
                    affected, behavior);
            }

            if (rows != queries.ExpectedRows && behavior == ConcurrencyTokenBehavior.ThrowException)
            {
                throw new DBConcurrencyException($"Expected rows to change {queries.ExpectedRows} but changed {rows}");
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

    public async Task<int> SaveChangesOptimizedAsync(DbContext context,
        QueryExecutionConfiguration? configuration,
        CancellationToken cancellationToken)
    {
        ILogger logger = _dbContextDependencyResolverService.GetLogger(context);

        configuration = Init(context, configuration);

        QueryPreparationModel queries = _queryPreparerService.Prepare(context, configuration);

        var autoCommit = false;

        IDbContextTransaction? transaction = context.Database.CurrentTransaction;

        if (configuration.AutoTransactionEnabled == true)
        {
            if (transaction == null)
            {
                IsolationLevel? isolationLevel = configuration.AutoTransactionIsolationLevel;

                if (!isolationLevel.HasValue)
                {
                    throw new ArgumentException("Auto transaction isolation level should be set");
                }

                transaction = await context.Database
                    .BeginTransactionAsync(isolationLevel.Value, cancellationToken)
                    .ConfigureAwait(false);

                autoCommit = true;
            }
        }

        if (transaction == null)
        {
            throw new ArgumentException(
                "There is no transaction on DbContext, enable auto transaction or attach transaction to DbContext");
        }

        var timeout = context.Database.GetCommandTimeout();

        try
        {
            var rows = 0;

            ConcurrencyTokenBehavior? behavior = configuration.ConcurrencyTokenBehavior;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (ISqlCommandModel sql in queries.Queries)
            {
                var affected = await _queryExecutorService
                    .ExecuteAsync(context, configuration, transaction, sql, timeout, cancellationToken)
                    .ConfigureAwait(false);

                rows += affected;

                if (affected >= 0 || behavior == ConcurrencyTokenBehavior.SaveWhatPossible)
                {
                    continue;
                }

                behavior = ConcurrencyTokenBehavior.SaveWhatPossible;

                logger.LogWarning(
                    "Returned {affected} affected rows, switch behavior to {behavior} for the current execution",
                    affected, behavior);
            }

            if (rows != queries.ExpectedRows && behavior == ConcurrencyTokenBehavior.ThrowException)
            {
                throw new DBConcurrencyException($"Expected rows to change {queries.ExpectedRows} but changed {rows}");
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

    private QueryExecutionConfiguration Init(DbContext context, QueryExecutionConfiguration? configuration)
    {
        var providerName = context.Database.ProviderName ?? throw new ArgumentException("Provider not known");

        configuration = _queryExecutionConfiguratorService.Get(providerName, configuration);

        _queryPreparerService.Init(context);

        return configuration;
    }

    private static void MarkEntitiesAfterSave(DbContext context)
    {
        foreach (EntityEntry entityEntry in context.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
        }
    }
}
