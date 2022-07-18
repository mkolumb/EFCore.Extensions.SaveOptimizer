using EFCore.Extensions.SaveOptimizer.Model.Cockroach;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

public static class WrapperResolver
{
    static WrapperResolver() => DbContextWrapper.TryInit(ContextWrapperResolver);

    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper? testOutputHelper,
        EntityCollectionAttribute? collectionAttribute)
    {
        CockroachDesignTimeFactory factory = new();

        const string truncateQuery = "truncate \"{0}\";";

        const string resetSequenceQuery = "select setval('\"{0}_{1}_seq\"', 1, false);";

        DbContextWrapper wrapper = new(factory, testOutputHelper, collectionAttribute, truncateQuery, resetSequenceQuery);

        try
        {
            wrapper.Migrate();

            wrapper.CleanDb();
        }
        catch
        {
            wrapper.Dispose();

            throw;
        }

        return wrapper;
    }
}
