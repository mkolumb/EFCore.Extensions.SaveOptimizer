using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseUpdateBenchmark : BaseBenchmark
{
    private int _iterations;

    public override string Operation => "Update";

    protected BaseUpdateBenchmark(IWrapperResolver contextResolver) : base(contextResolver)
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

        IReadOnlyList<NonRelatedEntity> items = Context.RetrieveData(Rows).GetAwaiter().GetResult();

        for (var i = 0L; i < Rows; i++)
        {
            items[(int)i].SomeNullableDecimalProperty = 9.181M + _iterations;
        }
    }

    [Benchmark(OperationsPerInvoke = 1)]
    public async Task UpdateAsync()
    {
        if (Context == null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        await Context.Save(Variant);
    }
}
