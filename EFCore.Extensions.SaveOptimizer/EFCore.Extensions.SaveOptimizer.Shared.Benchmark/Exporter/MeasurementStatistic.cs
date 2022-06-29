using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Common;
using Perfolizer.Horology;
using Perfolizer.Mathematics.QuantileEstimators;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

public class MeasurementStatisticColumn : IStatisticColumn
{
    public string Id => nameof(MeasurementStatisticColumn) + "." + ColumnName;

    public string ColumnName => "Measurement";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
        => Format(summary[benchmarkCase].ResultStatistics, SummaryStyle.Default);

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        => Format(summary[benchmarkCase].ResultStatistics, style);

    public bool IsAvailable(Summary summary) => true;
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Statistics;
    public int PriorityInCategory => 0;
    public bool IsNumeric => true;
    public UnitType UnitType => UnitType.Time;

    public string Legend => "Median from 0-75% range";

    public List<double> GetAllValues(Summary summary, SummaryStyle style)
        => summary.Reports
            .Where(r => r.ResultStatistics != null)
            .Select(r => GetCalculatedValue(r.ResultStatistics))
            .Where(v => !double.IsNaN(v) && !double.IsInfinity(v))
            .Select(v => UnitType == UnitType.Time ? v / style.TimeUnit.NanosecondAmount : v)
            .ToList();

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

    private static double GetCalculatedValue(Statistics? statistics)
    {
        if (statistics == null)
        {
            return double.NaN;
        }

        var values = statistics.OriginalValues
            .Where(s => s <= statistics.Q3)
            .ToArray();

        Quartiles quartiles = Quartiles.FromUnsorted(values);

        return quartiles.Median;
    }

    private string Format(Statistics? statistics, SummaryStyle style)
    {
        if (statistics == null)
        {
            return "NA";
        }

        const int precision = 4;
        var format = "N" + precision;

        var value = GetCalculatedValue(statistics);
        if (double.IsNaN(value))
        {
            return "NA";
        }

        return UnitType == UnitType.Time
            ? TimeInterval.FromNanoseconds(value)
                .ToString(
                    style.TimeUnit,
                    style.CultureInfo,
                    format,
                    UnitPresentation.FromVisibility(style.PrintUnitsInContent))
            : value.ToString(format, style.CultureInfo);
    }

    public override string ToString() => ColumnName;
}
