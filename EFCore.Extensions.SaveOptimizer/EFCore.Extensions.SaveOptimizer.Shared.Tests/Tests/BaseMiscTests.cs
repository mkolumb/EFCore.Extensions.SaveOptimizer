using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract partial class BaseMiscTests : BaseTests
{
    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => SharedTheoryData.BaseWriteTheoryData;

    public static IEnumerable<IEnumerable<object?>> TransactionTestTheoryData =>
        SharedTheoryData.TransactionTestTheoryData;

    protected BaseMiscTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    private static async Task<NonRelatedEntity[]> InitialSeedAsync(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await db.Context.AddAsync(ItemResolver(i)).ConfigureAwait(false);
        }

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        return await db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetryAsync()
            .ConfigureAwait(false);
    }

    private static NonRelatedEntity[] InitialSeed(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            db.Context.Add(ItemResolver(i));
        }

        db.Save(variant, null);

        return db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetry();
    }

    private static NonRelatedEntity ItemResolver(int i) =>
        new()
        {
            ConcurrencyToken = new DateTimeOffset(2033, 11, 11, 2, 3, 4, 5, TimeSpan.Zero),
            SomeNonNullableBooleanProperty = true,
            SomeNonNullableDateTimeProperty = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNullableDateTimeProperty = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNonNullableDecimalProperty = 2.52M,
            SomeNullableDecimalProperty = 4.523M,
            SomeNonNullableIntProperty = i,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = $"some-string-{i}",
            SomeNullableStringProperty = "other-string"
        };

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
                TestOutputHelper.WriteLineWithDate($"Retry number {i} {method.Method.Name}");

                TestOutputHelper.WriteLineWithDate(ex.Message);

                TestOutputHelper.WriteLineWithDate(ex.StackTrace);

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
                TestOutputHelper.WriteLineWithDate($"Retry number {i} {method.Method.Name}");

                TestOutputHelper.WriteLineWithDate(ex.Message);

                TestOutputHelper.WriteLineWithDate(ex.StackTrace);

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
