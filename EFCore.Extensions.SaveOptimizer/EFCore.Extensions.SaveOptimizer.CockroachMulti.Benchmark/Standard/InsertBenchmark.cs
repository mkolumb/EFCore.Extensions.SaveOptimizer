﻿using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark.Specific;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark.Standard;

public class InsertBenchmark : BaseInsertBenchmark
{
    public override string Database => Variables.DbName;

    [ParamsSource(nameof(ValuesForRows))]
    public override long Rows { get; set; }

    public IEnumerable<long> ValuesForRows => Variables.Rows;

    public InsertBenchmark() : base(BenchmarkHelper.ContextResolver())
    {
    }
}
