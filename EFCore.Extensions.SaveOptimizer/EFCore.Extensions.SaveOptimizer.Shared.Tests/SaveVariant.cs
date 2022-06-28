namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

[Flags]
public enum SaveVariant
{
    Optimized = 2,
    OptimizedDapper = 4,
    EfCore = 8,
    Recreate = 16,
    WithTransaction = 32
}
