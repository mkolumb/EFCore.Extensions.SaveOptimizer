using EFCore.Extensions.SaveOptimizer.Model.Postgres;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver()
    {
        PostgresDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory);

        wrapper.Context.Database.Migrate();

        const string query = "truncate \"NonRelatedEntities\";";

        wrapper.Context.Database.ExecuteSqlRaw(query);

        return wrapper;
    }
}
