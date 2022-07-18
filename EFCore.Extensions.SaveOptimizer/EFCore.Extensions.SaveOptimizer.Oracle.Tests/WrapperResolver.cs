using EFCore.Extensions.SaveOptimizer.Model.Oracle;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Oracle.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper, EntityCollectionAttribute? collectionAttribute)
    {
        OracleDesignTimeFactory factory = new();

        const string truncateQuery = "TRUNCATE TABLE \"{0}\";";

        const string resetSequenceQuery = "alter table \"{0}\" modify \"{1}\" generated always as identity restart start with 1;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, truncateQuery, resetSequenceQuery);

        wrapper.Migrate();

        wrapper.CleanDb(collectionAttribute);

        return wrapper;
    }
}
