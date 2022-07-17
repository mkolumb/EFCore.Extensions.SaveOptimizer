using EFCore.Extensions.SaveOptimizer.Model.SqlServer;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        SqlServerDesignTimeFactory factory = new();

        const string query = "truncate table \"{0}\";";

        DbContextWrapper wrapper = new(factory, testOutputHelper, query);

        wrapper.Context.Database.Migrate();

        wrapper.CleanDb();

        return wrapper;
    }
}
