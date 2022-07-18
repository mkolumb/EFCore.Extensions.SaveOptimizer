using EFCore.Extensions.SaveOptimizer.Model.Postgres;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Postgres.Tests;

public static class WrapperResolver
{
    static WrapperResolver() => DbContextWrapper.TryInit(ContextWrapperResolver);

    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper? testOutputHelper,
        EntityCollectionAttribute? collectionAttribute)
    {
        PostgresDesignTimeFactory factory = new();

        const string query = "truncate \"{0}\" RESTART IDENTITY;";

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
