namespace EFCore.Extensions.SaveOptimizer.Firebird4.Benchmark.Specific;

public class Variables
{
    public const string DbName = "Firebird4";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L };

    public static long[] InsertRows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L };
}
