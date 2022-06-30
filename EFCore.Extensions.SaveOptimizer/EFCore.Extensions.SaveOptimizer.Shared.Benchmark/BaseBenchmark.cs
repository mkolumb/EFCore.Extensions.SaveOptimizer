using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseBenchmark
{
    private const int MaxPrepareTry = 5;
    private readonly IWrapperResolver _contextResolver;
    protected IDbContextWrapper? Context;
    protected int Iterations;

    public abstract string Database { get; }

    public abstract string Operation { get; }

    public abstract long Rows { get; set; }

    [Params(SaveVariant.Optimized, SaveVariant.OptimizedDapper, SaveVariant.EfCore)]
    public SaveVariant Variant { get; set; }

    protected BaseBenchmark(IWrapperResolver contextResolver) => _contextResolver = contextResolver;

    [GlobalSetup]
    public async Task Setup()
    {
        ConsoleLogger.Unicode.WriteLineHint($"Setup {GetDescription()}");

        Context = _contextResolver.Resolve();

        await Context.Seed(Rows, BenchmarkConfig.GetSeedRepeat());
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Iterations++;

        ConsoleLogger.Unicode.WriteLineHint($"Iteration setup {Iterations} {GetDescription()}");

        var i = 0;

        while (i < MaxPrepareTry)
        {
            try
            {
                Prepare();

                return;
            }
            catch
            {
                i++;
            }
        }

        throw new Exception($"Unable to prepare iteration {Iterations} {GetDescription()}");
    }

    protected abstract void Prepare();

    [GlobalCleanup]
    public async Task Cleanup()
    {
        ConsoleLogger.Unicode.WriteLineHint($"Cleanup {GetDescription()}");

        if (Context != null)
        {
            await Context.Truncate();

            Context.Dispose();
        }
    }

    protected string GetDescription() => $"{Database} {Operation} {Variant} {Rows} {DateTimeOffset.UtcNow:T}";
}
