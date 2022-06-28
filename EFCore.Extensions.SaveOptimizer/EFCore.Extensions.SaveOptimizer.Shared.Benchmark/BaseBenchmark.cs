using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

[SimpleJob(RunStrategy.Monitoring, 5, 2, 10, 10)]
[MinColumn]
[MaxColumn]
[MeanColumn]
[MedianColumn]
[MarkdownExporterAttribute.GitHub]
[CsvMeasurementsExporter]
public abstract class BaseBenchmark
{
    private readonly IWrapperResolver _contextResolver;
    protected IDbContextWrapper? Context;

    public abstract string Database { get; }

    public abstract string Operation { get; }

    public abstract long Rows { get; set; }

    [Params(SaveVariant.Optimized, SaveVariant.OptimizedDapper, SaveVariant.EfCore)]
    public SaveVariant Variant { get; set; }

    protected BaseBenchmark(IWrapperResolver contextResolver) => _contextResolver = contextResolver;

    [GlobalSetup]
    public async Task Setup()
    {
        Console.WriteLine($"Setup {GetDescription()}");

        Context = _contextResolver.Resolve();

        await Context.Seed(Rows, 20);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        Console.WriteLine($"Cleanup {GetDescription()}");

        if (Context != null)
        {
            await Context.Truncate();

            Context.Dispose();
        }
    }

    protected string GetDescription() => $"{Database} {Operation} {Variant} {Rows} {DateTimeOffset.UtcNow:T}";
}
