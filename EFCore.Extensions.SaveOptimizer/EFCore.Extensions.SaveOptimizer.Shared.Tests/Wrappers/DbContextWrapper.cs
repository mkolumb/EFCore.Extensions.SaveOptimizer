using System.Data;
using EFCore.Extensions.SaveOptimizer.Dapper;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.TestLogger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;

public sealed class DbContextWrapper : IDisposable
{
    private const int RunTry = 10;

    private readonly ITestTimeDbContextFactory<EntitiesContext> _factory;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly string? _resetSequenceFormat;
    private readonly string? _truncateFormat;

    public EntitiesContext Context { get; private set; }

    public string[] EntitiesList { get; } =
    {
        nameof(EntitiesContext.NonRelatedEntities), nameof(EntitiesContext.AutoIncrementPrimaryKeyEntities),
        nameof(EntitiesContext.VariousTypeEntities), nameof(EntitiesContext.FailingEntities)
    };

    public Dictionary<string, string> SequencesList { get; } = new()
    {
        { nameof(EntitiesContext.AutoIncrementPrimaryKeyEntities), nameof(AutoIncrementPrimaryKeyEntity.Id) },
        { nameof(EntitiesContext.FailingEntities), nameof(FailingEntity.Id) }
    };

    public DbContextWrapper(ITestTimeDbContextFactory<EntitiesContext> factory, ITestOutputHelper testOutputHelper,
        string? truncateFormat = null, string? resetSequenceFormat = null)
    {
        _factory = factory;
        _truncateFormat = truncateFormat;
        _resetSequenceFormat = resetSequenceFormat;
        _loggerFactory = new LoggerFactory(new[] { new TestLoggerProvider(testOutputHelper, LogLevel.Warning) });

        Context = _factory.CreateDbContext(Array.Empty<string>(), _loggerFactory);
        _logger = _loggerFactory.CreateLogger(nameof(DbContextWrapper));
    }

    public void Dispose() => Context.Dispose();

    private static QueryExecutionConfiguration GetConfig(SaveVariant variant, int? batchSize)
    {
        QueryExecutionConfiguration configuration =
            batchSize.HasValue
                ? new QueryExecutionConfiguration { BatchSize = batchSize }
                : new QueryExecutionConfiguration();

        if (variant.HasFlag(SaveVariant.NoAutoTransaction))
        {
            configuration.AutoTransactionEnabled = false;
        }

        if (!variant.HasFlag(SaveVariant.WithTransaction))
        {
            configuration.AfterSaveBehavior = AfterSaveBehavior.AcceptChanges;
        }

        return configuration;
    }

    public void RecreateContext(string? connectionString = null)
    {
        connectionString ??= Context.Database.GetConnectionString();

        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        Context.Dispose();

        Context = _factory.CreateDbContext(new[] { connectionString }, _loggerFactory);
    }

    public async Task SaveAsync(SaveVariant variant, int? batchSize, int retries = RunTry)
    {
        await RunAsync(retries, () => TrySaveAsync(variant, batchSize)).ConfigureAwait(false);

        RecreateContext();
    }

    public void CleanDb()
    {
        if (_truncateFormat != null)
        {
            foreach (var entity in EntitiesList)
            {
                var query = string.Format(_truncateFormat, entity);

                Run(RunTry, () => Context.Database.ExecuteSqlRaw(query));
            }
        }

        if (_resetSequenceFormat == null)
        {
            return;
        }

        foreach (var (table, column) in SequencesList)
        {
            var query = string.Format(_resetSequenceFormat, table, column);

            Run(RunTry, () => Context.Database.ExecuteSqlRaw(query));
        }
    }

    private async Task TrySaveAsync(SaveVariant variant, int? batchSize)
    {
        QueryExecutionConfiguration configuration = GetConfig(variant, batchSize);

        if (variant.HasFlag(SaveVariant.WithTransaction))
        {
            IDbContextTransaction transaction =
                await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable).ConfigureAwait(false);

            try
            {
                IExecutionResultModel result = await SaveChangesAsync(variant, configuration).ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);

                result.ProcessAfterSave(AfterSaveBehavior.AcceptChanges);
            }
            catch (Exception ex)
            {
                _logger.LogWithDate("Error within transaction");

                _logger.LogWithDate(ex.Message);

                _logger.LogWithDate(ex.StackTrace);

                await transaction.RollbackAsync().ConfigureAwait(false);

                throw;
            }
            finally
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
            }
        }
        else
        {
            await SaveChangesAsync(variant, configuration).ConfigureAwait(false);
        }
    }

    public async Task<IExecutionResultModel> SaveChangesAsync(SaveVariant variant,
        QueryExecutionConfiguration configuration)
    {
        if (variant.HasFlag(SaveVariant.Optimized))
        {
            return await Context.SaveChangesOptimizedAsync(configuration).ConfigureAwait(false);
        }

        if (variant.HasFlag(SaveVariant.OptimizedDapper))
        {
            return await Context.SaveChangesDapperOptimizedAsync(configuration).ConfigureAwait(false);
        }

        // ReSharper disable once InvertIf
        if (variant.HasFlag(SaveVariant.EfCore))
        {
            var rows = await Context
                .SaveChangesAsync(configuration.AfterSaveBehavior == AfterSaveBehavior.AcceptChanges)
                .ConfigureAwait(false);

            return new ExecutionResultModel(rows, null, Context);
        }

        throw new Exception("Unexpected variant exception");
    }

    private async Task RunAsync(int max, Func<Task> method)
    {
        var i = 0;

        do
        {
            try
            {
                await method().ConfigureAwait(false);

                return;
            }
            catch (Exception ex)
            {
                _logger.LogWithDate($"Retry number {i} {method.Method.Name}");

                _logger.LogWithDate(ex.Message);

                _logger.LogWithDate(ex.StackTrace);

                i++;

                if (i >= max)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }

    public void Save(SaveVariant variant, int? batchSize, int retries = RunTry)
    {
        Run(retries, () => TrySave(variant, batchSize));

        RecreateContext();
    }

    private void TrySave(SaveVariant variant, int? batchSize)
    {
        QueryExecutionConfiguration configuration = GetConfig(variant, batchSize);

        if (variant.HasFlag(SaveVariant.WithTransaction))
        {
            IDbContextTransaction transaction = Context.Database.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                IExecutionResultModel result = SaveChanges(variant, configuration);

                transaction.Commit();

                result.ProcessAfterSave(AfterSaveBehavior.AcceptChanges);
            }
            catch (Exception ex)
            {
                _logger.LogWithDate("Error within transaction");

                _logger.LogWithDate(ex.Message);

                _logger.LogWithDate(ex.StackTrace);

                transaction.Rollback();

                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }
        else
        {
            SaveChanges(variant, configuration);
        }
    }

    public IExecutionResultModel SaveChanges(SaveVariant variant, QueryExecutionConfiguration configuration)
    {
        if (variant.HasFlag(SaveVariant.Optimized))
        {
            return Context.SaveChangesOptimized(configuration);
        }

        if (variant.HasFlag(SaveVariant.OptimizedDapper))
        {
            return Context.SaveChangesDapperOptimized(configuration);
        }

        // ReSharper disable once InvertIf
        if (variant.HasFlag(SaveVariant.EfCore))
        {
            var rows = Context.SaveChanges(configuration.AfterSaveBehavior == AfterSaveBehavior.AcceptChanges);

            return new ExecutionResultModel(rows, null, Context);
        }

        throw new Exception("Unexpected variant exception");
    }

    private void Run(int max, Action method)
    {
        var i = 0;

        do
        {
            try
            {
                method();

                return;
            }
            catch (Exception ex)
            {
                _logger.LogWithDate($"Retry number {i} {method.Method.Name}");

                _logger.LogWithDate(ex.Message);

                _logger.LogWithDate(ex.StackTrace);

                i++;

                if (i >= max)
                {
                    throw;
                }

                Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }
}
