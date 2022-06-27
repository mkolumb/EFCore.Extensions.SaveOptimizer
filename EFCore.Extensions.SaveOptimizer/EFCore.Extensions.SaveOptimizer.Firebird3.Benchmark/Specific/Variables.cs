namespace EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark.Specific;

public class Variables
{
    public const string DbName = "Firebird3";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L, 10000L };
}
