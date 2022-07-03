using EFCore.Extensions.SaveOptimizer.Model.CockroachMulti;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.CockroachMulti.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        CockroachDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        const string query = "truncate \"NonRelatedEntities\";";

        wrapper.Context.Database.ExecuteSqlRaw(query);

        return wrapper;
    }
}
