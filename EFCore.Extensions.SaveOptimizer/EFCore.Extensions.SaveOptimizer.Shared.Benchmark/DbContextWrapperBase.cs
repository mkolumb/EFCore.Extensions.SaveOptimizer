using System.Data;
using EFCore.Extensions.SaveOptimizer.Dapper;
using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class DbContextWrapperBase : IDbContextWrapper
{
    private readonly IDbContextFactory<EntitiesContext> _factory;

    protected DbContextWrapperBase(IDbContextFactory<EntitiesContext> factory)
    {
        _factory = factory;

        Context = _factory.CreateDbContext();
    }

    public EntitiesContext Context { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public abstract Task Truncate();

    public async Task Seed(long count, int repeat)
    {
        await Truncate();

        try
        {
            await TrySeed(count, repeat, IsolationLevel.ReadUncommitted);
        }
        catch
        {
            await TrySeed(count / 10, repeat * 10, IsolationLevel.ReadUncommitted);
        }

        await TrySeed(1, 1, IsolationLevel.Serializable);
    }

    private async Task TrySeed(long count, int repeat, IsolationLevel isolationLevel)
    {
        for (var j = 0; j < repeat / 10; j++)
        {
            for (var i = 0; i < count * 10; i++)
            {
                NonRelatedEntity item = CreateItem(i);

                await Context.AddAsync(item);
            }

            await SeedSave(isolationLevel);
        }
    }

    private async Task SeedSave(IsolationLevel isolationLevel)
    {
        try
        {
            await TrySave(SaveVariant.Optimized, isolationLevel);
        }
        catch
        {
            await TrySave(SaveVariant.Optimized, isolationLevel);
        }

        RecreateContext();
    }

    public async Task Save(SaveVariant variant)
    {
        try
        {
            await TrySave(variant, IsolationLevel.Serializable);
        }
        catch
        {
            await TrySave(variant, IsolationLevel.Serializable);
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
