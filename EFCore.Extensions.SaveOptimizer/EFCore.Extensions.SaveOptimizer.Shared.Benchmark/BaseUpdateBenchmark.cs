using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
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

        ConsoleLogger.Unicode.WriteLineHint($"Iteration setup {_iterations} {GetDescription()}");

        IReadOnlyList<NonRelatedEntity> items = Context.RetrieveData(Rows).GetAwaiter().GetResult();

        if (items.Count != Rows)
        {
            ConsoleLogger.Unicode.WriteLineError($"Expected {Rows} rows but retrieved {items.Count}");
        }

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
