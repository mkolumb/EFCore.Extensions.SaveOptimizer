using EFCore.Extensions.SaveOptimizer.Model.Firebird3;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Firebird3.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper)
    {
        Firebird3DesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory, testOutputHelper);

        wrapper.Context.Database.Migrate();

        const string query = "DELETE FROM \"NonRelatedEntities\";";

        wrapper.Context.Database.ExecuteSqlRaw(query);

        return wrapper;
    }
}
