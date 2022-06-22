namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

[Flags]
public enum SaveVariant
{
    Normal = 2,
    Recreate = 4,
    Optimized = 8,
    WithTransaction = 16
}
