using EFCore.Extensions.SaveOptimizer.Model.SqlLite;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        SqlLiteDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        return wrapper;
    }
}
