using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseDeleteBenchmark : BaseBenchmark
{
    private int _iterations;

    public override string Operation => "Delete";

    protected BaseDeleteBenchmark(IWrapperResolver contextResolver) : base(contextResolver)
    {
    }

    [IterationSetup]
    public void IterationSetup()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        _iterations++;

        ConsoleLogger.Unicode.WriteLineHint($"Iteration setup {_iterations} {GetDescription()}");

        IReadOnlyList<NonRelatedEntity> items = Context.RetrieveData(Rows).GetAwaiter().GetResult();

        if (items.Count != Rows)
        {
            ConsoleLogger.Unicode.WriteLineError($"Expected {Rows} rows but retrieved {items.Count}");
        }

        for (var i = 0L; i < Rows; i++)
        {
            Context.Context.Remove(items[(int)i]);
        }
    }

    [Benchmark(OperationsPerInvoke = 1)]
    public async Task DeleteAsync()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        await Context.Save(Variant);
    }
}
