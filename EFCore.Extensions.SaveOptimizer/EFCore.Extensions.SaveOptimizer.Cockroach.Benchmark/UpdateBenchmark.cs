﻿using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark;

public class UpdateBenchmark : BaseUpdateBenchmark
{
    public override string Database => "Cockroach";

    [Params(1L, 10L, 25L, 50L, 100L, 1000L, 10000L)]
    public override long Rows { get; set; }

    public UpdateBenchmark() : base(BenchmarkHelper.ContextResolver())
    {
    }
}