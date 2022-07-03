namespace EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark.Specific;

public class Variables
{
    public const string DbName = "SqlServer";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L, 10000L };
}
