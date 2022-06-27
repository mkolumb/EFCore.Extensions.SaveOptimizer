using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract class BaseUpdateTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Func<ITestOutputHelper, DbContextWrapper> ContextWrapperResolver { get; }

    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => TheoryData.BaseWriteTheoryData;

    protected BaseUpdateTests(
        ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
    {
        _testOutputHelper = testOutputHelper;
        ContextWrapperResolver = contextWrapperResolver;
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChanges_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver(_testOutputHelper);

        NonRelatedEntity[] data = await InitialSeed(db, variant, 10);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            entity.SomeNonNullableIntProperty++;
            entity.SomeNonNullableIntProperty--;
        }

        // Act
        await db.Save(variant);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayAsync();

        var newState = JsonConvert.SerializeObject(result);

        // Assert
        result.Should().HaveCount(10);

        newState.Should().BeEquivalentTo(state);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChanges_WhenOneObjectUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver(_testOutputHelper);

        NonRelatedEntity[] data = await InitialSeed(db, variant, 3);

        data[0].SomeNonNullableDecimalProperty = 191;

        // Act
        await db.Save(variant);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayAsync();

        // Assert
        result.Should().HaveCount(3);
        result[0].SomeNonNullableDecimalProperty.Should().Be(191);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[2].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChanges_WhenMultipleObjectsUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver(_testOutputHelper);

        NonRelatedEntity[] data = await InitialSeed(db, variant, 15);

        for (var i = 0; i < 5; i++)
        {
            data[i].SomeNullableIntProperty = i;
        }

        data[5].SomeNullableIntProperty = null;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 == 0)
            {
                data[i].SomeNonNullableDecimalProperty = 191;
            }
        }

        // Act
        await db.Save(variant);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayAsync();

        var nullableIntProperties = result.Select(x => x.SomeNullableIntProperty).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.SomeNonNullableDecimalProperty).ToArray();

        // Assert
        result.Should().HaveCount(15);
        nullableIntProperties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, null, 11, 11, 11, 11, 11, 11, 11, 11, 11);
        nonNullableDecimalProperties.Should()
            .ContainInOrder(191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M,
                2.52M);
    }

    private static async Task<NonRelatedEntity[]> InitialSeed(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await db.Context.AddAsync(ItemResolver(i));
        }

        await db.Save(variant);

        return await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayAsync();
    }

    private static NonRelatedEntity ItemResolver(int i) =>
        new()
        {
            ConcurrencyToken = new DateTimeOffset(2033, 11, 11, 2, 3, 4, 5, TimeSpan.Zero),
            SomeNonNullableBooleanProperty = true,
            SomeNonNullableDateTimeProperty = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNullableDateTimeProperty = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNonNullableDecimalProperty = 2.52M,
            SomeNullableDecimalProperty = 4.523M,
            SomeNonNullableIntProperty = 1,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = $"some-string-{i}",
            SomeNullableStringProperty = "other-string"
        };
}
