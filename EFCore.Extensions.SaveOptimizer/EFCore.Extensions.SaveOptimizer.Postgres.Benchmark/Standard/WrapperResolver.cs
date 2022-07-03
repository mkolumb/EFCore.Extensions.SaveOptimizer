using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Postgres.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Postgres.Benchmark.Standard;

public class WrapperResolver : IWrapperResolver
{
    private readonly IDbContextFactory<EntitiesContext> _factory;

    public WrapperResolver(IDbContextFactory<EntitiesContext> factory) => _factory = factory;

    public IDbContextWrapper Resolve()
    {
        DbContextWrapper wrapper = new(_factory);

        wrapper.Context.Database.Migrate();

        return wrapper;
    }
}
