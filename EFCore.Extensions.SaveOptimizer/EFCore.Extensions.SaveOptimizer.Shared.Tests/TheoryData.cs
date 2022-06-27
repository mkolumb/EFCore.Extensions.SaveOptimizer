namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public static class TheoryData
{
    public static IEnumerable<IEnumerable<object>> BaseWriteTheoryData
    {
        get
        {
            yield return new object[] { SaveVariant.EfCore | SaveVariant.Recreate };
            yield return new object[] { SaveVariant.EfCore | SaveVariant.Recreate | SaveVariant.WithTransaction };
            yield return new object[] { SaveVariant.Optimized | SaveVariant.Recreate };
            yield return new object[] { SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction };
        }
    }

    public static IEnumerable<IEnumerable<object>> InsertTheoryData
    {
        get
        {
            foreach (IEnumerable<object> baseData in BaseWriteTheoryData)
            {
                var item = baseData.First();

                yield return new[] { item, 1000, 1 };
                yield return new[] { item, 1000, 2 };
                yield return new[] { item, 1000, 10 };
                yield return new[] { item, 1000, 1000 };
            }

            yield return new object[]
            {
                SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction, 100000, 100000
            };
        }
    }
}
