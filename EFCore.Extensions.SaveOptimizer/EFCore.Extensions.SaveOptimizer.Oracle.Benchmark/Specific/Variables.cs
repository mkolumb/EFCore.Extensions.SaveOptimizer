namespace EFCore.Extensions.SaveOptimizer.Oracle.Benchmark.Specific;

public class Variables
{
    public const string DbName = "Oracle";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L, 10000L };

    public static long[] InsertRows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L };
}
