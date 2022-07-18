using System.Data;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Dapper;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class DbContextWrapperBase : IDbContextWrapper
{
    private const int RunTry = 10;
    private const int MaxSeed = 10000;
    private const int MaxFailures = 5;
    private readonly IDbContextFactory<EntitiesContext> _factory;
    private int _failures;

    protected string[] EntitiesList { get; } = { "NonRelatedEntities", "AutoIncrementPrimaryKeyEntities" };

    protected DbContextWrapperBase(IDbContextFactory<EntitiesContext> factory)
    {
        _failures = 0;

        _factory = factory;

        Context = _factory.CreateDbContext();
    }

    public EntitiesContext Context { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task TruncateAsync() => await RunAsync(RunTry, TruncateBaseAsync).ConfigureAwait(false);

    public async Task SeedAsync(long count, int repeat)
    {
        double delay = Math.Max(count / 1000, 5);

        while (count > MaxSeed)
        {
            count /= 10;
            repeat *= 10;
        }

        async Task InternalSeed()
        {
            try
            {
                await TrySeedAsync(count, repeat, IsolationLevel.ReadCommitted).ConfigureAwait(false);
            }
            catch
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(delay)).ConfigureAwait(false);

                    await TrySeedAsync(count / 10, repeat * 10, IsolationLevel.ReadCommitted).ConfigureAwait(false);
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(delay)).ConfigureAwait(false);

                    await TrySeedAsync(count / 100, repeat * 100, IsolationLevel.ReadCommitted).ConfigureAwait(false);
                }
            }
        }

        await TruncateAsync().ConfigureAwait(false);

        await RunAsync(RunTry, InternalSeed).ConfigureAwait(false);

        await RunAsync(RunTry, () => TrySeedAsync(1, 1, IsolationLevel.Serializable)).ConfigureAwait(false);
    }

    public async Task SaveAsync(SaveVariant variant, long expectedRows)
    {
        try
        {
            await RunAsync(RunTry, () => TrySaveAsync(variant, IsolationLevel.Serializable)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _failures++;

            if (_failures > MaxFailures)
            {
                throw new Exception(
                    $"Failures limit exceeded, rows {expectedRows}, variant {variant}, failures {_failures}");
            }

            double delay = Math.Max(expectedRows / 1000, 5);

            ConsoleLogger.Unicode.WriteLineWithDate(
                $"Unable to save, failure {_failures}, wait {delay} seconds to mark as outlier");

            ConsoleLogger.Unicode.WriteLineWithDate(ex.Message);

            ConsoleLogger.Unicode.WriteLineWithDate(ex.StackTrace);

            await Task.Delay(TimeSpan.FromSeconds(delay)).ConfigureAwait(false);
        }

        RecreateContext();
    }

    public async Task<IReadOnlyList<NonRelatedEntity>> RetrieveDataAsync(long count) =>
        await Context.NonRelatedEntities
            .Take((int)(count % int.MaxValue))
            .ToArrayAsync().ConfigureAwait(false);

    public NonRelatedEntity CreateItem(long i) => new()
    {
        ConcurrencyToken = RemoveMilliseconds(DateTimeOffset.UtcNow),
        SomeNonNullableBooleanProperty = true,
        SomeNonNullableDateTimeProperty = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
        SomeNullableDateTimeProperty = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
        SomeNonNullableDecimalProperty = 2.52M,
        SomeNullableDecimalProperty = 4.523M,
        SomeNonNullableIntProperty = 1,
        SomeNullableIntProperty = 11,
        SomeNonNullableStringProperty = $"some-string-{i}",
        SomeNullableStringProperty = "other-string"
    };

    public void Migrate() => Context.Database.Migrate();

    private static async Task RunAsync(int max, Func<Task> method)
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
                ConsoleLogger.Unicode.WriteLineWithDate($"Retry number {i} {method.Method.Name}");

                ConsoleLogger.Unicode.WriteLineWithDate(ex.Message);

                ConsoleLogger.Unicode.WriteLineWithDate(ex.StackTrace);

                i++;

                if (i >= max)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.Zero).ConfigureAwait(false);
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }

    protected abstract Task TruncateBaseAsync();

    private async Task TrySeedAsync(long count, int repeat, IsolationLevel isolationLevel)
    {
        ConsoleLogger.Unicode.WriteLineWithDate($"Try seed {count} * {repeat} / {isolationLevel}");

        for (var j = 0; j < Math.Max(repeat, 1); j++)
        {
            await TrySeedOnceAsync(Math.Max(count, 1), isolationLevel).ConfigureAwait(false);
        }
    }

    private async Task TrySeedOnceAsync(long count, IsolationLevel isolationLevel)
    {
        ConsoleLogger.Unicode.WriteLineWithDate($"Try seed once {count} / {isolationLevel}");

        async Task InternalTrySeedOnce()
        {
            for (var i = 0; i < count; i++)
            {
                NonRelatedEntity item = CreateItem(i);

                await Context.AddAsync(item).ConfigureAwait(false);
            }

            await SeedSaveAsync(isolationLevel).ConfigureAwait(false);
        }

        await RunAsync(RunTry, InternalTrySeedOnce).ConfigureAwait(false);
    }

    private async Task SeedSaveAsync(IsolationLevel isolationLevel)
    {
        await RunAsync(RunTry, () => TrySaveAsync(SaveVariant.Optimized, isolationLevel)).ConfigureAwait(false);

        RecreateContext();
    }

    private async Task TrySaveAsync(SaveVariant variant, IsolationLevel isolationLevel)
    {
        using CancellationTokenSource source = new(TimeSpan.FromMinutes(10));

        CancellationToken token = source.Token;

        async Task InternalSave()
        {
            switch (variant)
            {
                case SaveVariant.Optimized:
                    await Context.SaveChangesOptimizedAsync(token).ConfigureAwait(false);
                    break;
                case SaveVariant.OptimizedDapper:
                    await Context.SaveChangesDapperOptimizedAsync(token).ConfigureAwait(false);
                    break;
                case SaveVariant.EfCore:
                    await Context.SaveChangesAsync(token).ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variant), variant, null);
            }
        }

        IDbContextTransaction transaction =
            await Context.Database.BeginTransactionAsync(isolationLevel, token).ConfigureAwait(false);
        await using ConfiguredAsyncDisposable _ = transaction.ConfigureAwait(false);

        await InternalSave().ConfigureAwait(false);

        await transaction.CommitAsync(token).ConfigureAwait(false);
    }

    private static DateTimeOffset? RemoveMilliseconds(DateTimeOffset x) =>
        new DateTimeOffset(x.Year, x.Month, x.Day, x.Hour, x.Minute, x.Second, 0, x.Offset);

    private void RecreateContext()
    {
        var connectionString = Context.Database.GetConnectionString();

        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        Context.Dispose();

        Context = _factory.CreateDbContext();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Context.Dispose();
        }
    }
}
