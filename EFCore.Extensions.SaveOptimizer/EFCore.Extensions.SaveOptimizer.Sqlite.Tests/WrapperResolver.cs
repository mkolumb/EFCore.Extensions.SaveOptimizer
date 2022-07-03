using EFCore.Extensions.SaveOptimizer.Model.Sqlite;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Sqlite.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        SqliteDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        return wrapper;
    }
}
