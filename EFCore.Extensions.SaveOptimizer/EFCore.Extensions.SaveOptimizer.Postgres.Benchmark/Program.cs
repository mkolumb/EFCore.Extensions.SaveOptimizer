using System.Reflection;
using BenchmarkDotNet.Running;
using EFCore.Extensions.SaveOptimizer.Postgres.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

namespace EFCore.Extensions.SaveOptimizer.Postgres.Benchmark;

public static class Program
{
    public static void Main(string[] args) =>
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(), new BenchmarkConfig(Variables.DbName));
}
