using EFCore.Extensions.SaveOptimizer.Model.CockroachMulti;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.CockroachMulti.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper, EntityCollectionAttribute? collectionAttribute)
    {
        CockroachDesignTimeFactory factory = new();

        const string truncateQuery = "truncate \"{0}\";";

        const string resetSequenceQuery = "select setval('\"{0}_{1}_seq\"', 1, false);";

        DbContextWrapper wrapper = new(factory, testOutputHelper, truncateQuery, resetSequenceQuery);

        wrapper.Migrate();

        wrapper.CleanDb(collectionAttribute);

        return wrapper;
    }
}
