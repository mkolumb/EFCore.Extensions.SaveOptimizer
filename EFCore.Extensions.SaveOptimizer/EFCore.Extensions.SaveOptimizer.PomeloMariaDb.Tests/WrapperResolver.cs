using EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        PomeloMariaDbDesignTimeFactory factory = new();

        const string query = "truncate `{0}`;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, query);

        wrapper.Context.Database.Migrate();

        wrapper.CleanDb();

        return wrapper;
    }
}
