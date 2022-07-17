using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark.Specific;

public class DbContextWrapper : DbContextWrapperBase
{
    public DbContextWrapper(IDbContextFactory<EntitiesContext> factory) : base(factory)
    {
    }

    protected override async Task TruncateBaseAsync()
    {
        foreach (var entity in EntitiesList)
        {
            var query = $"truncate `{entity}`;";

            await Context.Database.ExecuteSqlRawAsync(query).ConfigureAwait(false);
        }
    }
}
