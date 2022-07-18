using EFCore.Extensions.SaveOptimizer.Model.Sqlite;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Sqlite.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper, EntityCollectionAttribute? collectionAttribute)
    {
        SqliteDesignTimeFactory factory = new();

        const string truncateQuery = "DELETE FROM \"{0}\";";

        DbContextWrapper wrapper = new(factory, testOutputHelper, truncateQuery);

        wrapper.Migrate();

        wrapper.CleanDb(collectionAttribute);

        return wrapper;
    }
}
