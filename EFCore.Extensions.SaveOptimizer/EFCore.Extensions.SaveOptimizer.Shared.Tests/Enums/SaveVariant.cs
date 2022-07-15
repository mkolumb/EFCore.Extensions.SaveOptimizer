namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;

[Flags]
public enum SaveVariant
{
    Default = 0,
    Optimized = 1,
    OptimizedDapper = 2,
    EfCore = 4,
    Recreate = 8,
    WithTransaction = 16,
    NoAutoTransaction = 32
}
