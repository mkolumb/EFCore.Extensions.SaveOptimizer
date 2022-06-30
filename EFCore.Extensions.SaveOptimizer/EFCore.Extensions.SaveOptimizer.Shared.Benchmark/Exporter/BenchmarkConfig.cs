using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using Perfolizer.Mathematics.OutlierDetection;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

public class BenchmarkConfig : ManualConfig
{
    private const int InvocationCount = 1;
    private const int IterationCount = 3;
    private const int LaunchCount = 25;
    private const int WarmupCount = 2;
    private const int UnrollFactor = 1;

    public BenchmarkConfig(string dbName)
    {
        Job job = new(dbName)
        {
            Run =
            {
                InvocationCount = InvocationCount,
                IterationCount = IterationCount,
                LaunchCount = LaunchCount,
                WarmupCount = WarmupCount,
                UnrollFactor = UnrollFactor,
                RunStrategy = RunStrategy.Throughput
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
