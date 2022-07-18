using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Model.Entities;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseInsertBenchmark : BaseBenchmark
{
    public override string Operation => "Insert";

    protected BaseInsertBenchmark(IWrapperResolver contextResolver) : base(contextResolver)
    {
    }

    protected override void Prepare()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        for (var i = 0L; i < Rows; i++)
        {
            NonRelatedEntity entity = Context.CreateItem(i);

            Context.Context.NonRelatedEntities.Add(entity);
        }
    }

    [Benchmark(OperationsPerInvoke = 1)]
    public async Task InsertAsync()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        await Context.SaveAsync(Variant, Rows).ConfigureAwait(false);
    }
}
