using System.Data;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Dapper;
using EFCore.Extensions.SaveOptimizer.Model;
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

    public async Task Truncate() => await Run(RunTry, TruncateBase);

    public async Task Seed(long count, int repeat)
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
                await TrySeed(count, repeat, IsolationLevel.ReadCommitted);
            }
            catch
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(delay));

                    await TrySeed(count / 10, repeat * 10, IsolationLevel.ReadCommitted);
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(delay));

                    await TrySeed(count / 100, repeat * 100, IsolationLevel.ReadCommitted);
                }
            }
        }

        await Truncate();

        await Run(RunTry, InternalSeed);

        await Run(RunTry, () => TrySeed(1, 1, IsolationLevel.Serializable));
    }

    public async Task Save(SaveVariant variant, long expectedRows)
    {
        try
        {
            await Run(RunTry, () => TrySave(variant, IsolationLevel.Serializable));
        }
        catch (Exception ex)
        {
            _failures++;

            if (_failures > MaxFailures)
            {
                throw new Exception($"Failures limit exceeded, rows {expectedRows}, variant {variant}, failures {_failures}");
            }

            double delay = Math.Max(expectedRows / 1000, 5);

            ConsoleLogger.Unicode.WriteLineHint($"Unable to save, failure {_failures}, wait {delay} seconds to mark as outlier");

            ConsoleLogger.Unicode.WriteLineHint(ex.Message);

            ConsoleLogger.Unicode.WriteLineHint(ex.StackTrace);

            await Task.Delay(TimeSpan.FromSeconds(delay));
        }

        RecreateContext();
    }

    public async Task<IReadOnlyList<NonRelatedEntity>> RetrieveData(long count) =>
        await Context.NonRelatedEntities
            .Take((int)(count % int.MaxValue))
            .ToArrayAsync();

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

    private static async Task Run(int max, Func<Task> method)
    {
        var i = 0;

        while (i < max)
        {
            try
            {
                await method();

                return;
            }
            catch
            {
                ConsoleLogger.Unicode.WriteLineHint($"Retry number {i} {method.Method.Name}");

                await Task.Delay(TimeSpan.Zero);

                i++;
            }
        }

        throw new Exception("Unable to run method");
    }

    protected abstract Task TruncateBase();

    private async Task TrySeed(long count, int repeat, IsolationLevel isolationLevel)
    {
        ConsoleLogger.Unicode.WriteLineHint($"Try seed {count} * {repeat} / {isolationLevel}");

        for (var j = 0; j < Math.Max(repeat, 1); j++)
        {
            await TrySeedOnce(Math.Max(count, 1), isolationLevel);
        }
    }

    private async Task TrySeedOnce(long count, IsolationLevel isolationLevel)
    {
        async Task InternalTrySeedOnce()
        {
            for (var i = 0; i < count; i++)
            {
                NonRelatedEntity item = CreateItem(i);

                await Context.AddAsync(item);
            }

            await SeedSave(isolationLevel);
        }

        await Run(RunTry, InternalTrySeedOnce);
    }

    private async Task SeedSave(IsolationLevel isolationLevel)
    {
        await Run(RunTry, () => TrySave(SaveVariant.Optimized, isolationLevel));

        RecreateContext();
    }

    private async Task TrySave(SaveVariant variant, IsolationLevel isolationLevel)
    {
        using CancellationTokenSource source = new(TimeSpan.FromMinutes(10));

        CancellationToken token = source.Token;

        async Task InternalSave()
        {
            switch (variant)
            {
                case SaveVariant.Optimized:
                    await Context.SaveChangesOptimizedAsync(token);
                    break;
                case SaveVariant.OptimizedDapper:
                    await Context.SaveChangesDapperOptimizedAsync(token);
                    break;
                case SaveVariant.EfCore:
                    await Context.SaveChangesAsync(token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variant), variant, null);
            }
        }

        await using IDbContextTransaction transaction =
            await Context.Database.BeginTransactionAsync(isolationLevel, token);

        await InternalSave();

        await transaction.CommitAsync(token);
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
