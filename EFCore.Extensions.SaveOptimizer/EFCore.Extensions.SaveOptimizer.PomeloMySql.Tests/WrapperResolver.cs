using EFCore.Extensions.SaveOptimizer.Model.PomeloMySql;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.PomeloMySql.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        PomeloMySqlDesignTimeFactory factory = new();

        const string query = "truncate `{0}`;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, query);

        wrapper.Context.Database.Migrate();

        wrapper.CleanDb();

        return wrapper;
    }
}
