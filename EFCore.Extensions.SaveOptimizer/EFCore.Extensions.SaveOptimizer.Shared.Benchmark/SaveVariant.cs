namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

[Flags]
public enum SaveVariant
{
    Optimized = 2,
    EfCore = 4,
    Recreate = 8,
    WithTransaction = 16
}
