namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

[Flags]
public enum SaveVariant
{
    Normal = 2,
    Recreate = 4,
    Optimized = 8,
    WithTransaction = 16
}
