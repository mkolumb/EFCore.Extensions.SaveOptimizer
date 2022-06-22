using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public interface IDbContextWrapper : IDisposable
{
    EntitiesContext Context { get; }
    void RecreateContext();
    Task Truncate();
    Task Save(SaveVariant variant);
}
