using EFCore.Extensions.SaveOptimizer.Model.Postgres;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Postgres.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper, EntityCollectionAttribute? collectionAttribute)
    {
        PostgresDesignTimeFactory factory = new();

        const string query = "truncate \"{0}\" RESTART IDENTITY;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, query);

        wrapper.Migrate();

        wrapper.CleanDb(collectionAttribute);

        return wrapper;
    }
}
