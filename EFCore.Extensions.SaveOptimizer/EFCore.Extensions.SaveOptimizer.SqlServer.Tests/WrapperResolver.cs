using EFCore.Extensions.SaveOptimizer.Model.SqlServer;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver()
    {
        SqlServerDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory);

        wrapper.Context.Database.Migrate();

        const string query = "truncate table \"NonRelatedEntities\";";

        wrapper.Context.Database.ExecuteSqlRaw(query);

        return wrapper;
    }
}
