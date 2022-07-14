using EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        PomeloMariaDbDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        const string query = "truncate `{0}`;";

        wrapper.CleanDb(query);

        return wrapper;
    }
}
