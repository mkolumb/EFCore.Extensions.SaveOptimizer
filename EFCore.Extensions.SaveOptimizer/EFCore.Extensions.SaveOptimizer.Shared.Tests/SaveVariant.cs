namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

[Flags]
public enum SaveVariant
{
    Optimized = 2,
    EfCore = 4,
    Recreate = 8,
    WithTransaction = 16
}
