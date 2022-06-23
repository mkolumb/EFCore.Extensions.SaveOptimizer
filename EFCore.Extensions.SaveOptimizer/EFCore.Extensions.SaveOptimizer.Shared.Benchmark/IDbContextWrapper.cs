using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public interface IDbContextWrapper : IDisposable
{
    EntitiesContext Context { get; }
    Task Truncate();
    Task Save(SaveVariant variant);
}
