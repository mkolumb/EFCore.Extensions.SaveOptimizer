using EFCore.Extensions.SaveOptimizer.Model.Oracle21;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Oracle21.Tests;

public static class WrapperResolver
{
    static WrapperResolver() => DbContextWrapper.TryInit(ContextWrapperResolver);

    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper? testOutputHelper,
        EntityCollectionAttribute? collectionAttribute)
    {
        Oracle21DesignTimeFactory factory = new();

        const string truncateQuery = "TRUNCATE TABLE \"{0}\";";

        const string resetSequenceQuery = "alter table \"{0}\" modify \"{1}\" generated always as identity restart start with 1;";

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
