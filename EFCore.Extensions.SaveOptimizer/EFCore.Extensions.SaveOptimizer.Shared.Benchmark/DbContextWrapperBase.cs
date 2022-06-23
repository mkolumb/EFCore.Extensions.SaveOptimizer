using System.Data;
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

    public async Task Seed(long count)
    {
        await Truncate();

        for (var i = 0; i < count; i++)
        {
            NonRelatedEntity item = CreateItem(i);

            await Context.AddAsync(item);
        }

        await Save(SaveVariant.Optimized | SaveVariant.WithTransaction | SaveVariant.Recreate);
    }

    public async Task Save(SaveVariant variant)
    {
        async Task InternalSave()
        {
            if ((variant & SaveVariant.Optimized) != 0)
            {
                await Context.SaveChangesOptimizedAsync();
            }
            else if ((variant & SaveVariant.EfCore) != 0)
            {
                await Context.SaveChangesAsync();
            }
        }

        if ((variant & SaveVariant.WithTransaction) != 0)
        {
            await using IDbContextTransaction transaction =
                await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            await InternalSave();

            await transaction.CommitAsync();
        }
        else
        {
            await InternalSave();
        }

        if ((variant & SaveVariant.Recreate) != 0)
        {
            RecreateContext();
        }
    }

    public async Task<IReadOnlyList<Guid>> RetrieveIds(long count) =>
        await Context.NonRelatedEntities
            .Select(x => x.NonRelatedEntityId)
            .Take((int)(count % int.MaxValue))
            .ToArrayAsync();

    public async Task<IReadOnlyList<NonRelatedEntity>> RetrieveData(long count)
    {
        IReadOnlyCollection<Guid> ids = await RetrieveIds(count);

        return await Context.NonRelatedEntities
            .Where(x => ids.Contains(x.NonRelatedEntityId))
            .ToArrayAsync();
    }

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
