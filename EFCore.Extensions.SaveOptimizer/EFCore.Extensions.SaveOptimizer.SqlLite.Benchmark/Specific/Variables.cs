namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark.Specific;

public class Variables
{
    public const string DbName = "SqlLite";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L, 10000L };
}
