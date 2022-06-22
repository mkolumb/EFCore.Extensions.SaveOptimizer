using BenchmarkDotNet.Running;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark;

public static class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run<InsertBenchmark>();
}
