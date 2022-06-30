using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using Perfolizer.Mathematics.OutlierDetection;

// ReSharper disable HeuristicUnreachableCode

#pragma warning disable CS0162

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

public class BenchmarkConfig : ManualConfig
{
    private const int InvocationCount = 1;
    private const int IterationCount = 20;
    private const int LaunchCount = 3;
    private const int WarmupCount = 0;
    private const int UnrollFactor = 1;

    public BenchmarkConfig(string dbName)
    {
        RunStrategy strategy = RunStrategy.ColdStart;

        if (WarmupCount > 0)
        {
            strategy = RunStrategy.Monitoring;
        }

        Job job = new(dbName)
        {
            Run =
            {
                InvocationCount = InvocationCount,
                IterationCount = IterationCount,
                LaunchCount = LaunchCount,
                WarmupCount = WarmupCount,
                UnrollFactor = UnrollFactor,
                RunStrategy = strategy
            },
            Accuracy = { EvaluateOverhead = true, OutlierMode = OutlierMode.RemoveUpper }
        };

        AddJob(job);

        AddColumnProvider(DefaultColumnProviders.Descriptor,
            DefaultColumnProviders.Job,
            DefaultColumnProviders.Params,
            DefaultColumnProviders.Metrics);

        AddColumn(new MeasurementStatisticColumn(),
            StatisticColumn.Mean,
            StatisticColumn.Min,
            StatisticColumn.Q1,
            StatisticColumn.Median,
            StatisticColumn.Q3,
            StatisticColumn.Max);

        AddExporter(MarkdownExporter.GitHub, CsvMeasurementsExporter.Default);

        AddLogger(ConsoleLogger.Unicode);
    }

    public static int GetSeedRepeat() => (InvocationCount * (IterationCount + WarmupCount) * UnrollFactor) + 1;
}
