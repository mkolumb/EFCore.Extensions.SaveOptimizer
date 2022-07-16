using EFCore.Extensions.SaveOptimizer.Model.Firebird4;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Firebird4.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        Firebird4DesignTimeFactory factory = new();

        const string truncateQuery = "DELETE FROM \"{0}\";";

        const string resetSequenceQuery = "ALTER TABLE \"{0}\" ALTER COLUMN \"{1}\" RESTART WITH 1;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, truncateQuery, resetSequenceQuery);

        wrapper.Context.Database.Migrate();

        wrapper.CleanDb();

        return wrapper;
    }
}
