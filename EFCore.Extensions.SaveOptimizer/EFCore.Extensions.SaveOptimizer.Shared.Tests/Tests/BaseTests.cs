using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseTests
{
    private const int MaxPrepareTry = 10;

    private readonly Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> _contextWrapperResolver;

    protected readonly EntityCollectionAttribute? CollectionAttribute;

    protected readonly ITestOutputHelper TestOutputHelper;

    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => SharedTheoryData.BaseWriteTheoryData;

    public static IEnumerable<IEnumerable<object?>> InsertData => SharedTheoryData.InsertTheoryData;

    public static IEnumerable<IEnumerable<object?>> TransactionTestTheoryData =>
        SharedTheoryData.TransactionTestTheoryData;

    public static ConcurrentDictionary<string, int> ContextFails { get; set; } = new();

    protected BaseTests(
        ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
    {
        TestOutputHelper = testOutputHelper;
        _contextWrapperResolver = contextWrapperResolver;

        var attributes = GetType().GetCustomAttributes(typeof(EntityCollectionAttribute), true);

        foreach (EntityCollectionAttribute? attribute in attributes)
        {
            if (attribute == null)
            {
                continue;
            }

            CollectionAttribute = attribute;
        }
    }

    public DbContextWrapper ContextWrapperResolver()
    {
        var name = CollectionAttribute?.CollectionName ?? Guid.NewGuid().ToString();

        ContextFails.TryAdd(name, 0);

        do
        {
            try
            {
                var wrapper = _contextWrapperResolver(TestOutputHelper, CollectionAttribute);

                ContextFails[name] = 0;

                return wrapper;
            }
            catch (Exception ex)
            {
                TestOutputHelper.WriteLineWithDate($"Error when creating context, try {ContextFails[name]}");

                TestOutputHelper.WriteLineWithDate(ex.Message);

                TestOutputHelper.WriteLineWithDate(ex.StackTrace);

                ContextFails[name]++;

                if (ContextFails[name] < MaxPrepareTry)
                {
                    Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
        } while (ContextFails[name] < MaxPrepareTry);

        throw new Exception("Unable to create context");
    }

    protected static async Task<NonRelatedEntity[]> InitialSeedAsync(DbContextWrapper db, SaveVariant variant,
        int count, Action<NonRelatedEntity>? setter = null)
    {
        for (var i = 0; i < count; i++)
        {
            NonRelatedEntity item = ItemResolver(i);

            setter?.Invoke(item);

            await db.Context.AddAsync(item).ConfigureAwait(false);
        }

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        return await db.Context.NonRelatedEntities
            .OrderBy(x => x.Indexer)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);
    }

    protected static NonRelatedEntity[] InitialSeed(DbContextWrapper db, SaveVariant variant, int count,
        Action<NonRelatedEntity>? setter = null)
    {
        for (var i = 0; i < count; i++)
        {
            NonRelatedEntity item = ItemResolver(i);

            setter?.Invoke(item);

            db.Context.Add(item);
        }

        db.Save(variant, null);

        return db.Context.NonRelatedEntities
            .OrderBy(x => x.Indexer)
            .ToArrayWithRetry();
    }

    protected static NonRelatedEntity ItemResolver(int i) =>
        new()
        {
            ConcurrencyToken = new DateTimeOffset(2033, 11, 11, 2, 3, 4, 5, TimeSpan.Zero),
            NonNullableBoolean = true,
            NonNullableDateTime = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            NullableDateTime = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            NonNullableDecimal = 2.52M,
            NullableDecimal = 4.523M,
            NonNullableInt = i,
            NullableInt = 11,
            NonNullableString = $"some-string-{i}",
            NullableString = "other-string",
            Indexer = i
        };

    protected async Task RunAsync(int max, Func<Task> method)
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
                TestOutputHelper.WriteLineWithDate($"Retry number {i} {method.Method.Name}");

                TestOutputHelper.WriteLineWithDate(ex.Message);

                TestOutputHelper.WriteLineWithDate(ex.StackTrace);

                i++;

                if (i >= max)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }

    protected void Run(int max, Action method)
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
                TestOutputHelper.WriteLineWithDate($"Retry number {i} {method.Method.Name}");

                TestOutputHelper.WriteLineWithDate(ex.Message);

                TestOutputHelper.WriteLineWithDate(ex.StackTrace);

                i++;

                if (i >= max)
                {
                    throw;
                }

                Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }
}
