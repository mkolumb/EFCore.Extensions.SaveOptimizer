﻿using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark;

public class DeleteBenchmark : BaseDeleteBenchmark
{
    public override string Database => "SqlLite";

    [Params(1L, 10L, 25L, 50L, 100L, 1000L)]
    public override long Rows { get; set; }

    public DeleteBenchmark() : base(BenchmarkHelper.ContextResolver())
    {
    }
}
