﻿namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark.Specific;

public class Variables
{
    public const string DbName = "Cockroach";

    public static long[] Rows { get; } = { 1L, 10L, 25L, 50L, 100L, 1000L, 10000L };
}
