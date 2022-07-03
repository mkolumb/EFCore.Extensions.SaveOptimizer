using System.Reflection;
using BenchmarkDotNet.Running;
using EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

namespace EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark;

public static class Program
{
    public static void Main(string[] args) =>
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(), new BenchmarkConfig(Variables.DbName));
}
