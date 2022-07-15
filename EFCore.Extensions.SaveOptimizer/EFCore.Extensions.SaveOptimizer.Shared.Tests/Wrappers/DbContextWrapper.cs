using System.Data;
using EFCore.Extensions.SaveOptimizer.Dapper;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
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

    public EntitiesContext Context { get; private set; }

    public string[] EntitiesList { get; } =
    {
        nameof(EntitiesContext.NonRelatedEntities), nameof(EntitiesContext.AutoIncrementPrimaryKeyEntities)
    };

    public Dictionary<string, string> SequencesList { get; } = new()
    {
        { nameof(EntitiesContext.AutoIncrementPrimaryKeyEntities), nameof(AutoIncrementPrimaryKeyEntity.Id) }
    };

    public DbContextWrapper(ITestTimeDbContextFactory<EntitiesContext> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _loggerFactory = new LoggerFactory(new[] { new TestLoggerProvider(testOutputHelper, LogLevel.Warning) });

        Context = _factory.CreateDbContext(Array.Empty<string>(), _loggerFactory);
        _logger = _loggerFactory.CreateLogger(nameof(DbContextWrapper));
    }

    public void Dispose() => Context.Dispose();

    public void RecreateContext()
    {
        var connectionString = Context.Database.GetConnectionString();

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

        if (variant.HasFlag(SaveVariant.Recreate))
        {
            RecreateContext();
        }
    }

    public void CleanDb(string truncateFormat, string? resetSequenceFormat = null)
    {
        foreach (var entity in EntitiesList)
        {
            var query = string.Format(truncateFormat, entity);

            Run(RunTry, () => Context.Database.ExecuteSqlRaw(query));
        }

        if (resetSequenceFormat == null)
        {
            return;
        }

        foreach (var (table, column) in SequencesList)
        {
            var query = string.Format(resetSequenceFormat, table, column);

            Run(RunTry, () => Context.Database.ExecuteSqlRaw(query));
        }
    }

    private async Task TrySaveAsync(SaveVariant variant, int? batchSize)
    {
        QueryExecutionConfiguration? configuration =
            batchSize.HasValue ? new QueryExecutionConfiguration { BatchSize = batchSize } : null;

        if (variant.HasFlag(SaveVariant.NoAutoTransaction))
        {
            configuration ??= new QueryExecutionConfiguration();

            configuration.AutoTransactionEnabled = false;
        }

        if (variant.HasFlag(SaveVariant.WithTransaction))
        {
            configuration ??= new QueryExecutionConfiguration();

            configuration.AcceptAllChangesOnSuccess = false;
        }

        async Task InternalSave()
        {
            if (variant.HasFlag(SaveVariant.Optimized))
            {
                await Context.SaveChangesOptimizedAsync(configuration).ConfigureAwait(false);
            }
            else if (variant.HasFlag(SaveVariant.OptimizedDapper))
            {
                await Context.SaveChangesDapperOptimizedAsync(configuration).ConfigureAwait(false);
            }
            else if (variant.HasFlag(SaveVariant.EfCore))
            {
                await Context.SaveChangesAsync(configuration?.AcceptAllChangesOnSuccess == true).ConfigureAwait(false);
            }
        }

        if (variant.HasFlag(SaveVariant.WithTransaction))
        {
            IDbContextTransaction transaction =
                await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable).ConfigureAwait(false);

            try
            {
                await InternalSave().ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);

                Context.ChangeTracker.AcceptAllChanges();
            }
            catch
            {
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
            await InternalSave().ConfigureAwait(false);
        }
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

        if (variant.HasFlag(SaveVariant.Recreate))
        {
            RecreateContext();
        }
    }

    private void TrySave(SaveVariant variant, int? batchSize)
    {
        QueryExecutionConfiguration? configuration =
            batchSize.HasValue ? new QueryExecutionConfiguration { BatchSize = batchSize } : null;

        if (variant.HasFlag(SaveVariant.NoAutoTransaction))
        {
            configuration ??= new QueryExecutionConfiguration();

            configuration.AutoTransactionEnabled = false;
        }

        if (variant.HasFlag(SaveVariant.WithTransaction))
        {
            configuration ??= new QueryExecutionConfiguration();

            configuration.AcceptAllChangesOnSuccess = false;
        }

        void InternalSave()
        {
            if (variant.HasFlag(SaveVariant.Optimized))
            {
                Context.SaveChangesOptimized(configuration);
            }
            else if (variant.HasFlag(SaveVariant.OptimizedDapper))
            {
                Context.SaveChangesDapperOptimized(configuration);
            }
            else if (variant.HasFlag(SaveVariant.EfCore))
            {
                Context.SaveChanges(configuration?.AcceptAllChangesOnSuccess == true);
            }
        }

        if (variant.HasFlag(SaveVariant.WithTransaction))
        {
            IDbContextTransaction transaction = Context.Database.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                InternalSave();

                transaction.Commit();

                Context.ChangeTracker.AcceptAllChanges();
            }
            catch
            {
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
            InternalSave();
        }
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
