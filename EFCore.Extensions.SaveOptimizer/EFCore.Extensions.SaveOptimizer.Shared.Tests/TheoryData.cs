namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public static class TheoryData
{
    private const string TestLoadMode = "TEST_LOAD_MODE";
    private const string TestFullLoadDisabledProviders = "TEST_FULL_LOAD_DISABLED_PROVIDERS";
    private const string FullLoadValue = "FULL";

    private static bool IsFullMode => TestDataHelper.GetValue(TestLoadMode) == FullLoadValue;

    private static IEnumerable<string> DisabledFullLoadProviders =>
        TestDataHelper.GetValues(TestFullLoadDisabledProviders);

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
                    yield return new object[] { framework | SaveVariant.Recreate | transaction };
                }
            }
        }
    }

    public static IEnumerable<IEnumerable<object?>> InsertTheoryData =>
        IsFullMode ? GetInsertFullMode() : GetInsertLightMode();

    private static IEnumerable<IEnumerable<object?>> GetInsertLightMode()
    {
        int?[] batches = { 100, default };

        foreach (IEnumerable<object> baseData in BaseWriteTheoryData)
        {
            var item = baseData.First();

            foreach (var batch in batches)
            {
                yield return new[] { item, batch, 1 };
                yield return new[] { item, batch, 2 };
                yield return new[] { item, batch, 10 };
                yield return new[] { item, batch, 100 };
            }
        }
    }

    private static IEnumerable<IEnumerable<object?>> GetInsertFullMode()
    {
        int?[] batches = { 1000, 100, 10, 1, default };

        foreach (IEnumerable<object> baseData in BaseWriteTheoryData)
        {
            var item = baseData.First();

            foreach (var batch in batches)
            {
                yield return new[] { item, batch, 1 };
                yield return new[] { item, batch, 2 };
                yield return new[] { item, batch, 10 };
                yield return new[] { item, batch, 100 };
                yield return new[] { item, batch, 1000 };
            }
        }

        if (TestDataHelper.IsDisabled(DisabledFullLoadProviders))
        {
            yield break;
        }

        var heavyLoadCounters = new[] { 10000, 100000 };

        foreach (var counter in heavyLoadCounters)
        {
            yield return new object?[]
            {
                SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction, counter, counter
            };

            yield return new object?[]
            {
                SaveVariant.OptimizedDapper | SaveVariant.Recreate | SaveVariant.WithTransaction, counter, counter
            };
        }
    }
}
