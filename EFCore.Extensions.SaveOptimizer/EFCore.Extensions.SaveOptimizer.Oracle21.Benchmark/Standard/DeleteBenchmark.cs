using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Oracle21.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Oracle21.Benchmark.Standard;

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
