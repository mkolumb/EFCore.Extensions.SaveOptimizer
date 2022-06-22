using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Context;
using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Services;

public class TranslatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TranslatorTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    [Fact]
    public void TranslatorShouldBeFast()
    {
        // Arrange
        GetData(out DataContextModelWrapper? wrapper, out EntityEntry[] changes);

        QueryTranslatorService translator = new();

        // Act
        const int howManyTimes = 5;

        List<TimeSpan> elapsedTimes = new();

        var count = 0;

        for (var i = 0; i < howManyTimes; i++)
        {
            DateTime start = DateTime.Now;

            count += changes.Select(x => translator.Translate(wrapper, x)).ToArray().Length;

            DateTime end = DateTime.Now;

            elapsedTimes.Add(end - start);
        }

        TimeSpan average = TimeSpan.FromMilliseconds(elapsedTimes.Average(x => x.TotalMilliseconds));

        // Assert
        _testOutputHelper.WriteLine($"average count: {count / howManyTimes}, average elapsed: {average}");

        average.Should().BeLessThan(TimeSpan.FromSeconds(15));
    }

    private static void GetData(out DataContextModelWrapper wrapper, out EntityEntry[] changes)
    {
        DbContextOptionsBuilder<TestDataContext> options = new();
        options = options.UseInMemoryDatabase("in_memory_db");

        TestDataContext context = new(options.Options);

        wrapper = new DataContextModelWrapper(() => context);

        changes = DataHelper.ResolveData(context);
    }
}
