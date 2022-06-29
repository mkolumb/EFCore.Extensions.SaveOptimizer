using System.Reflection;
using BenchmarkDotNet.Running;
using EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

namespace EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark;

public static class Program
{
    public static void Main(string[] args) =>
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(), new BenchmarkConfig(Variables.DbName));
}
