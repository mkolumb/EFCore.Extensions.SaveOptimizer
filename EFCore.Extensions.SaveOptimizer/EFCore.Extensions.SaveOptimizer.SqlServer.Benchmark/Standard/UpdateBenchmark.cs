using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark.Specific;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark.Standard;

public class UpdateBenchmark : BaseUpdateBenchmark
{
    public override string Database => Variables.DbName;

    [ParamsSource(nameof(ValuesForRows))]
    public override long Rows { get; set; }

    public IEnumerable<long> ValuesForRows => Variables.Rows;

    public UpdateBenchmark() : base(BenchmarkHelper.ContextResolver())
    {
    }
}
