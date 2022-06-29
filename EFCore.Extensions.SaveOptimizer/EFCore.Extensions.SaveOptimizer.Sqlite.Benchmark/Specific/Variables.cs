namespace EFCore.Extensions.SaveOptimizer.Sqlite.Benchmark.Specific;

public class Variables
{
    public const string DbName = "Sqlite";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L, 10000L };
}
