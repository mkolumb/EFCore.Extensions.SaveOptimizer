using BenchmarkDotNet.Running;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<DeleteBenchmark>();
        BenchmarkRunner.Run<UpdateBenchmark>();
        BenchmarkRunner.Run<InsertBenchmark>();
    }
}
