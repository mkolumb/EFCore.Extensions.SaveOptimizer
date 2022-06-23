using BenchmarkDotNet.Running;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<DeleteBenchmark>();
        BenchmarkRunner.Run<UpdateBenchmark>();
        BenchmarkRunner.Run<InsertBenchmark>();
    }
}
