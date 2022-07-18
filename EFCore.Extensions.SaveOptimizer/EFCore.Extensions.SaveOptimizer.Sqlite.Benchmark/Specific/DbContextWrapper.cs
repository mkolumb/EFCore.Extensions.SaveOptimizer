using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Sqlite.Benchmark.Specific;

public class DbContextWrapper : DbContextWrapperBase
{
    public DbContextWrapper(IDbContextFactory<EntitiesContext> factory) : base(factory)
    {
    }

    protected override async Task TruncateBaseAsync()
    {
        foreach (var entity in EntitiesList)
        {
            var query = $"DELETE FROM {entity}";

            await Context.Database.ExecuteSqlRawAsync(query).ConfigureAwait(false);
        }
    }
}
