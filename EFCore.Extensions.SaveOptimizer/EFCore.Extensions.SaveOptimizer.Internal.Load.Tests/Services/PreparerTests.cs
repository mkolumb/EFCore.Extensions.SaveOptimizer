using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Context;
using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Resolvers;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#pragma warning disable CS8602

namespace EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Services;

public class PreparerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PreparerTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    [Fact]
    public void PreparerShouldBeFast()
    {
        // Arrange
        GetData(out TestDataContext? context,
            out QueryCompilerService? queryCompiler,
            out QueryTranslatorService queryTranslator);

        QueryPreparerService queryPreparer = new(queryCompiler, queryTranslator);

        queryPreparer.Init(context);

        // Act
        const int howManyTimes = 5;

        List<TimeSpan> elapsedTimes = new();

        var count = 0;

        for (var i = 0; i < howManyTimes; i++)
        {
            DateTime start = DateTime.Now;

            count += queryPreparer.Prepare(context).ToArray().Length;

            DateTime end = DateTime.Now;

            elapsedTimes.Add(end - start);
        }

        TimeSpan average = TimeSpan.FromMilliseconds(elapsedTimes.Average(x => x.TotalMilliseconds));

        // Assert
        _testOutputHelper.WriteLine($"average count: {count / howManyTimes}, average elapsed: {average}");

        average.Should().BeLessThan(TimeSpan.FromSeconds(15));
    }

    private static void GetData(out TestDataContext context,
        out QueryCompilerService queryCompiler,
        out QueryTranslatorService translator)
    {
        translator = new QueryTranslatorService();

        DbContextOptionsBuilder<TestDataContext> options = new();
        options = options.UseInMemoryDatabase("in_memory_db");

        context = new TestDataContext(options.Options);
        TestDataContext dataContext = context;
        DataContextModelWrapper wrapper = new(() => dataContext);

        EntityEntry[] changes = DataHelper.ResolveData(context);

        QueryTranslatorService service = translator;

        QueryDataModel?[] queries = changes.Select(x => service.Translate(wrapper, x)).ToArray();

        Dictionary<EntityState, QueryDataModel?[]> batches = queries.GroupBy(x => x.EntityState)
            .ToDictionary(x => x.Key, x => x.ToArray());

        CompilerWrapperResolver resolver = new();

        queryCompiler = new QueryCompilerService(resolver);
    }
}
