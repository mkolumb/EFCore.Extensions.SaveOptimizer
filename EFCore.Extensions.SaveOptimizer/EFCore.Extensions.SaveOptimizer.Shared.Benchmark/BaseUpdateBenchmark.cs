using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Model.Entities;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseUpdateBenchmark : BaseBenchmark
{
    public override string Operation => "Update";

    protected BaseUpdateBenchmark(IWrapperResolver contextResolver) : base(contextResolver)
    {
    }

    protected override void Prepare()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        IReadOnlyList<NonRelatedEntity> items = Context.RetrieveDataAsync(Rows).ConfigureAwait(false).GetAwaiter().GetResult();

        if (items.Count != Rows)
        {
            ConsoleLogger.Unicode.WriteLineError($"Expected {Rows} rows but retrieved {items.Count}");
        }

        for (var i = 0L; i < Rows; i++)
        {
            items[(int)i].NullableDecimal = 9.181M + Iterations;
        }
    }

    [Benchmark(OperationsPerInvoke = 1)]
    public async Task UpdateAsync()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        await Context.SaveAsync(Variant, Rows).ConfigureAwait(false);
    }
}
