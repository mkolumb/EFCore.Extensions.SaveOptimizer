namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public interface IWrapperResolver
{
    DbContextWrapper Resolve();
}
