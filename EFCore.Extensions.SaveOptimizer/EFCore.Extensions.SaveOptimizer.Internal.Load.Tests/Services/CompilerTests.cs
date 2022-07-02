using EFCore.Extensions.SaveOptimizer.Internal.Factories;
using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Context;
using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#pragma warning disable CS8620
#pragma warning disable CS8602

namespace EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Services;

public class CompilerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CompilerTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    [Fact]
    public void CompilerShouldBeFast()
    {
        // Arrange
        GetData(out Dictionary<EntityState, QueryDataModel?[]> batches, out IQueryBuilderFactory factory);

        QueryCompilerService queryCompiler = new(factory);

        // Act
        const int howManyTimes = 5;

        List<TimeSpan> insertElapsedTimes = new();

        var insertCount = 0;

        List<TimeSpan> updateElapsedTimes = new();

        var updateCount = 0;

        List<TimeSpan> deleteElapsedTimes = new();

        var deleteCount = 0;

        for (var i = 0; i < howManyTimes; i++)
        {
            DateTime insertStart = DateTime.Now;

            _testOutputHelper.WriteLine($"start insert {insertStart}");

            insertCount += queryCompiler.Compile(batches[EntityState.Added], "InMemory", null).Count();

            DateTime insertEnd = DateTime.Now;

            insertElapsedTimes.Add(insertEnd - insertStart);

            DateTime updateStart = DateTime.Now;

            _testOutputHelper.WriteLine($"start update {updateStart}");

            updateCount += queryCompiler.Compile(batches[EntityState.Modified], "InMemory", null).Count();

            DateTime updateEnd = DateTime.Now;

            updateElapsedTimes.Add(updateEnd - updateStart);

            DateTime deleteStart = DateTime.Now;

            _testOutputHelper.WriteLine($"start delete {deleteStart}");

            deleteCount += queryCompiler.Compile(batches[EntityState.Deleted], "InMemory", null).Count();

            DateTime deleteEnd = DateTime.Now;

            deleteElapsedTimes.Add(deleteEnd - deleteStart);
        }

        TimeSpan averageInsert = TimeSpan.FromMilliseconds(insertElapsedTimes.Average(x => x.TotalMilliseconds));

        TimeSpan averageUpdate = TimeSpan.FromMilliseconds(updateElapsedTimes.Average(x => x.TotalMilliseconds));

        TimeSpan averageDelete = TimeSpan.FromMilliseconds(deleteElapsedTimes.Average(x => x.TotalMilliseconds));

        // Assert
        _testOutputHelper.WriteLine(
            $"insert average count: {insertCount / howManyTimes}, average elapsed: {averageInsert}");

        averageInsert.Should().BeLessThan(TimeSpan.FromSeconds(30));

        _testOutputHelper.WriteLine(
            $"update average count: {updateCount / howManyTimes}, average elapsed: {averageUpdate}");

        averageUpdate.Should().BeLessThan(TimeSpan.FromSeconds(30));

        _testOutputHelper.WriteLine(
            $"delete average count: {deleteCount / howManyTimes}, average elapsed: {averageDelete}");

        averageDelete.Should().BeLessThan(TimeSpan.FromSeconds(30));
    }

    private static void GetData(out Dictionary<EntityState, QueryDataModel?[]> batches,
        out IQueryBuilderFactory factory)
    {
        QueryTranslatorService translator = new();

        DbContextOptionsBuilder<TestDataContext> options = new();
        options = options.UseInMemoryDatabase("in_memory_db");

        TestDataContext context = new(options.Options);
        DataContextModelWrapper wrapper = new(() => context);

        EntityEntry[] changes = DataHelper.ResolveData(context);

        QueryDataModel?[] queries = changes.Select(x => translator.Translate(wrapper, x)).ToArray();

        batches = queries.GroupBy(x => x.EntityState)
            .ToDictionary(x => x.Key, x => x.ToArray());

        factory = new QueryBuilderFactory();
    }
}
