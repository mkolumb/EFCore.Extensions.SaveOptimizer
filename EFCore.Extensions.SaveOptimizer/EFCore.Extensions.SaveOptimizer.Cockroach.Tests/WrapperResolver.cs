using EFCore.Extensions.SaveOptimizer.Model.Cockroach;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        CockroachDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        const string truncateQuery = "truncate \"{0}\";";

        const string resetSequenceQuery = "select setval('\"{0}_{1}_seq\"', 1, false);";

        wrapper.CleanDb(truncateQuery, resetSequenceQuery);

        return wrapper;
    }
}
