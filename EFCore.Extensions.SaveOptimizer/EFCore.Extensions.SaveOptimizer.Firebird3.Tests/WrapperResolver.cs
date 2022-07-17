using EFCore.Extensions.SaveOptimizer.Model.Firebird3;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Firebird3.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        Firebird3DesignTimeFactory factory = new();

        const string truncateQuery = "DELETE FROM \"{0}\";";

        const string resetSequenceQuery = "ALTER TABLE \"{0}\" ALTER COLUMN \"{1}\" RESTART WITH 0;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, truncateQuery, resetSequenceQuery);

        wrapper.Context.Database.Migrate();

        wrapper.CleanDb();

        return wrapper;
    }
}
