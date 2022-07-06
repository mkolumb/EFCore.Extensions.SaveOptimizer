using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseDeleteBenchmark : BaseBenchmark
{
    public override string Operation => "Delete";

    protected BaseDeleteBenchmark(IWrapperResolver contextResolver) : base(contextResolver)
    {
    }

    protected override void Prepare()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        IReadOnlyList<NonRelatedEntity> items = Context.RetrieveDataAsync(Rows).GetAwaiter().GetResult();

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

        await Context.SaveAsync(Variant, Rows);
    }
}
