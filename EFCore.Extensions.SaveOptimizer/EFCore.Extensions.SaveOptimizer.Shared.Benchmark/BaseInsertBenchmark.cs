using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseInsertBenchmark : BaseBenchmark
{
    private int _iterations;

    public override string Operation => "Insert";

    protected BaseInsertBenchmark(IWrapperResolver contextResolver) : base(contextResolver)
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

        await Context.Save(Variant);
    }
}
