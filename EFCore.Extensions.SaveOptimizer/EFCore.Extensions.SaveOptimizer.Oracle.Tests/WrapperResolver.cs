using EFCore.Extensions.SaveOptimizer.Model.Oracle;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Oracle.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        OracleDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        const string truncateQuery = "TRUNCATE TABLE \"{0}\";";

        const string resetSequenceQuery = "alter table \"{0}\" modify \"{1}\" generated always as identity restart start with 1;";

        wrapper.CleanDb(truncateQuery, resetSequenceQuery);

        return wrapper;
    }
}
