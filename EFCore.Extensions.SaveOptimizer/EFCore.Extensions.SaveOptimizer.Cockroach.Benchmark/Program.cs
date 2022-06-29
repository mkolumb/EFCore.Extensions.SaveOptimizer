using System.Reflection;
using BenchmarkDotNet.Running;
using EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark;

public static class Program
{
    public static void Main(string[] args) =>
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(), new BenchmarkConfig(Variables.DbName));
}
