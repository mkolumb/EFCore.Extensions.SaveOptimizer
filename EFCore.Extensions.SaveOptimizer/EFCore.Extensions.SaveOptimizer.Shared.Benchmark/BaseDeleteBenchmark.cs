using BenchmarkDotNet.Attributes;
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

        Console.WriteLine($"Iteration setup {_iterations} {GetDescription()}");

        IReadOnlyList<NonRelatedEntity> items = Context.RetrieveData(Rows).GetAwaiter().GetResult();

        if (items.Count != Rows)
        {
            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine($"Expected {Rows} rows but retrieved {items.Count}");

            Console.ForegroundColor = oldColor;
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
