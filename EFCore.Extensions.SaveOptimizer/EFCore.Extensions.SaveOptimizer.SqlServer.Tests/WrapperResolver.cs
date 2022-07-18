using EFCore.Extensions.SaveOptimizer.Model.SqlServer;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Tests;

public static class WrapperResolver
{
    static WrapperResolver() => DbContextWrapper.TryInit(ContextWrapperResolver);

    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper? testOutputHelper,
        EntityCollectionAttribute? collectionAttribute)
    {
        SqlServerDesignTimeFactory factory = new();

        const string query = "truncate table \"{0}\";";

        DbContextWrapper wrapper = new(factory, testOutputHelper, collectionAttribute, query);

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
