using System.Reflection;
using BenchmarkDotNet.Running;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark;

public static class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run(Assembly.GetExecutingAssembly());
}
