using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public interface IDbContextWrapper : IDisposable
{
    EntitiesContext Context { get; }
    Task Truncate();
    Task Seed(long count, int repeat);
    Task Save(SaveVariant variant);
    Task<IReadOnlyList<Guid>> RetrieveIds(long count);
    Task<IReadOnlyList<NonRelatedEntity>> RetrieveData(long count);
    NonRelatedEntity CreateItem(long i);
}
