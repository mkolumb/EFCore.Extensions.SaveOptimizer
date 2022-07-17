using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;

public static class SharedTheoryData
{
    private const string TestLoadMode = "TEST_LOAD_MODE";
    private const string TestFullLoadDisabledProviders = "TEST_FULL_LOAD_DISABLED_PROVIDERS";
    private const string FullLoadValue = "FULL";

    private static bool IsFullMode => TestDataHelper.GetValue(TestLoadMode) == FullLoadValue;

    private static IEnumerable<string> DisabledFullLoadProviders =>
        TestDataHelper.GetValues(TestFullLoadDisabledProviders);

    public static IEnumerable<IEnumerable<object>> TransactionTestTheoryData
    {
        get
        {
            SaveVariant[] frameworks = { SaveVariant.EfCore, SaveVariant.Optimized, SaveVariant.OptimizedDapper };

            AfterSaveBehavior[] behaviors =
            {
                AfterSaveBehavior.MarkTemporaryAsPermanent, AfterSaveBehavior.AcceptChanges,
                AfterSaveBehavior.ClearChanges, AfterSaveBehavior.DetachSaved, AfterSaveBehavior.DoNothing
            };

            foreach (SaveVariant framework in frameworks)
            {
                foreach (AfterSaveBehavior behavior in behaviors)
                {
                    yield return new object[] { framework, behavior };
                }
            }
        }
    }

    public static IEnumerable<IEnumerable<object>> BaseWriteTheoryData
    {
        get
        {
            SaveVariant[] frameworks = { SaveVariant.EfCore, SaveVariant.Optimized, SaveVariant.OptimizedDapper };

            SaveVariant[] transactions =
            {
                SaveVariant.Default, SaveVariant.WithTransaction,
                SaveVariant.WithTransaction | SaveVariant.NoAutoTransaction
            };

            foreach (SaveVariant framework in frameworks)
            {
                foreach (SaveVariant transaction in transactions)
                {
                    yield return new object[] { framework | transaction };
                }
            }
        }
    }

    public static IEnumerable<IEnumerable<object?>> InsertTheoryData =>
        IsFullMode ? GetInsertFullMode() : GetInsertLightMode();

    private static IEnumerable<IEnumerable<object?>> GetInsertLightMode()
    {
        int?[] batches = { default, 1, 100 };

        int[] loadCounters = { 1, 2, 10, 100 };

        foreach (IEnumerable<object> baseData in BaseWriteTheoryData)
        {
            var item = baseData.First();

            foreach (var batch in batches)
            {
                foreach (var counter in loadCounters)
                {
                    yield return new[] { item, batch, counter };
                }
            }
        }
    }

    private static IEnumerable<IEnumerable<object?>> GetInsertFullMode()
    {
        int?[] batches = { default, 1, 10, 100, 1000 };

        int[] normalLoadCounters = { 1, 2, 10, 100, 1000 };

        int[] heavyLoadCounters = { 10000, 100000 };

        foreach (IEnumerable<object> baseData in BaseWriteTheoryData)
        {
            var item = baseData.First();

            foreach (var batch in batches)
            {
                foreach (var counter in normalLoadCounters)
                {
                    yield return new[] { item, batch, counter };
                }
            }
        }

        if (TestDataHelper.IsDisabled(DisabledFullLoadProviders))
        {
            yield break;
        }

        foreach (var counter in heavyLoadCounters)
        {
            yield return new object?[] { SaveVariant.Optimized | SaveVariant.WithTransaction, counter, counter };

            yield return new object?[] { SaveVariant.OptimizedDapper | SaveVariant.WithTransaction, counter, counter };
        }
    }
}
