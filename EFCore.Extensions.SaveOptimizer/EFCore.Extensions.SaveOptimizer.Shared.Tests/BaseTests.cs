using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract class BaseTests
{
    private const int MaxPrepareTry = 5;

    protected readonly ITestOutputHelper TestOutputHelper;

    private readonly Func<ITestOutputHelper, DbContextWrapper> _contextWrapperResolver;

    protected BaseTests(
        ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
    {
        TestOutputHelper = testOutputHelper;
        _contextWrapperResolver = contextWrapperResolver;
    }

    public DbContextWrapper ContextWrapperResolver()
    {
        var i = 0;

        while (i < MaxPrepareTry)
        {
            try
            {
                return _contextWrapperResolver(TestOutputHelper);
            }
            catch (Exception ex)
            {
                TestOutputHelper.WriteLineWithDate($"Error when creating context, try {i}");

                TestOutputHelper.WriteLineWithDate(ex.Message);

                TestOutputHelper.WriteLineWithDate(ex.StackTrace);

                Task.Delay(TimeSpan.FromSeconds(15)).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            i++;
        }

        throw new Exception("Unable to create context");
    }
}
