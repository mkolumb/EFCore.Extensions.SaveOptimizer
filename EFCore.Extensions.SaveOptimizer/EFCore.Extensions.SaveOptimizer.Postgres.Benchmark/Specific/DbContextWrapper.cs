using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Postgres.Benchmark.Specific;

public class DbContextWrapper : DbContextWrapperBase
{
    public DbContextWrapper(IDbContextFactory<EntitiesContext> factory) : base(factory)
    {
    }

    public override async Task Truncate()
    {
        const string query = "truncate \"NonRelatedEntities\";";

        await Context.Database.ExecuteSqlRawAsync(query);
    }
}
