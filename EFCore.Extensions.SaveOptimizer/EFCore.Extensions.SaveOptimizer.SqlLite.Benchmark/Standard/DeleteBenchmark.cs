using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark.Specific;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark.Standard;

public class DeleteBenchmark : BaseDeleteBenchmark
{
    public override string Database => Variables.DbName;

    [ParamsSource(nameof(ValuesForRows))]
    public override long Rows { get; set; }

    public IEnumerable<long> ValuesForRows => Variables.Rows;

    public DeleteBenchmark() : base(BenchmarkHelper.ContextResolver())
    {
    }
}
