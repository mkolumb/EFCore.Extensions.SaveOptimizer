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
    public BenchmarkConfig(string dbName)
    {
        Job job = new(dbName)
        {
            Run =
            {
                InvocationCount = 12,
                IterationCount = 12,
                LaunchCount = 8,
                WarmupCount = 2,
                UnrollFactor = 1,
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
}
