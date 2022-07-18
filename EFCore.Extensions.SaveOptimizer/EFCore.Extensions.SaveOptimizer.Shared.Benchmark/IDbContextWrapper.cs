using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Model.Entities;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public interface IDbContextWrapper : IDisposable
{
    EntitiesContext Context { get; }
    Task TruncateAsync();
    Task SeedAsync(long count, int repeat);
    Task SaveAsync(SaveVariant variant, long expectedRows);
    Task<IReadOnlyList<NonRelatedEntity>> RetrieveDataAsync(long count);
    NonRelatedEntity CreateItem(long i);
    void Migrate();
}
