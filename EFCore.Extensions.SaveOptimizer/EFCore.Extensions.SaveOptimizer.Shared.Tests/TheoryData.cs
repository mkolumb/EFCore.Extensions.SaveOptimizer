namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public static class TheoryData
{
    private static bool IsLightMode => Environment.GetEnvironmentVariables().Contains("TEST_LOAD_MODE") &&
                                       Environment.GetEnvironmentVariable("TEST_LOAD_MODE") == "LIGHT";

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

    public static IEnumerable<IEnumerable<object?>> InsertTheoryData
    {
        get
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

                    if (IsLightMode)
                    {
                        continue;
                    }

                    yield return new[] { item, batch, 1000 };
                }
            }

            if (IsLightMode)
            {
                yield break;
            }

            var excludedProviders = new[] { "Firebird", "Oracle" };

            if (excludedProviders.Any(p => AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName!.Contains(p))))
            {
                yield break;
            }

            yield return new object?[]
            {
                SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction, 100000, 100000
            };

            yield return new object?[]
            {
                SaveVariant.OptimizedDapper | SaveVariant.Recreate | SaveVariant.WithTransaction, 100000, 100000
            };
        }
    }
}
