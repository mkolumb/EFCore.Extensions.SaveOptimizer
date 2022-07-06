using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract class BaseDeleteTests : BaseTests
{
    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => TheoryData.BaseWriteTheoryData;

    protected BaseDeleteTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 10);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            db.Context.NonRelatedEntities.Remove(entity);
            db.Context.NonRelatedEntities.Update(entity);
        }

        // Act
        await db.SaveAsync(variant, null);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetryAsync();

        var newState = JsonConvert.SerializeObject(result);

        // Assert
        result.Should().HaveCount(10);

        newState.Should().BeEquivalentTo(state);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenOneObjectDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 3);

        db.Context.NonRelatedEntities.Remove(data[0]);

        // Act
        await db.SaveAsync(variant, null);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetryAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenMultipleObjectsDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 15);

        for (var i = 0; i < 5; i++)
        {
            db.Context.NonRelatedEntities.Remove(data[i]);
        }

        // Act
        await db.SaveAsync(variant, null);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetryAsync();

        var nonNullableIntProperties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(10);
        nonNullableIntProperties.Should().ContainInOrder(5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 10);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            db.Context.NonRelatedEntities.Remove(entity);
            db.Context.NonRelatedEntities.Update(entity);
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetry();

        var newState = JsonConvert.SerializeObject(result);

        // Assert
        result.Should().HaveCount(10);

        newState.Should().BeEquivalentTo(state);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenOneObjectDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 3);

        db.Context.NonRelatedEntities.Remove(data[0]);

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(2);
        result[0].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenMultipleObjectsDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 15);

        for (var i = 0; i < 5; i++)
        {
            db.Context.NonRelatedEntities.Remove(data[i]);
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetry();

        var nonNullableIntProperties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(10);
        nonNullableIntProperties.Should().ContainInOrder(5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
    }

    private static async Task<NonRelatedEntity[]> InitialSeedAsync(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await db.Context.AddAsync(ItemResolver(i));
        }

        await db.SaveAsync(variant, null);

        return await db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetryAsync();
    }

    private static NonRelatedEntity[] InitialSeed(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            db.Context.Add(ItemResolver(i));
        }

        db.Save(variant, null);

        return db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetry();
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
            SomeNonNullableIntProperty = i,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = $"some-string-{i}",
            SomeNullableStringProperty = "other-string"
        };
}
