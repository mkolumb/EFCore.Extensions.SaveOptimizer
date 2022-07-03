using System.Reflection;
using BenchmarkDotNet.Running;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;
using EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark.Specific;

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark;

public static class Program
{
    public static void Main(string[] args) =>
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(), new BenchmarkConfig(Variables.DbName));
}
