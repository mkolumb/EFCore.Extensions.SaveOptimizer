using EFCore.Extensions.SaveOptimizer.Model.Firebird3;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Firebird3.Tests;

public static class WrapperResolver
{
    static WrapperResolver() => DbContextWrapper.TryInit(ContextWrapperResolver);

    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper? testOutputHelper,
        EntityCollectionAttribute? collectionAttribute)
    {
        Firebird3DesignTimeFactory factory = new();

        const string truncateQuery = "DELETE FROM \"{0}\";";

        const string resetSequenceQuery = "ALTER TABLE \"{0}\" ALTER COLUMN \"{1}\" RESTART WITH 0;";

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
