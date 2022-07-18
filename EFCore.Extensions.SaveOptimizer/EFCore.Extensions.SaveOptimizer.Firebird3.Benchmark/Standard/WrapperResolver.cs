using EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using EFCore.Extensions.SaveOptimizer.Model.Context;

namespace EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark.Standard;

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
