using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using Xunit.Abstractions;

// ReSharper disable AccessToDisposedClosure

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract partial class BaseMiscTests : BaseTests
{
    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => TheoryData.BaseWriteTheoryData;

    protected BaseMiscTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [Theory]
    [InlineData(SaveVariant.Optimized | SaveVariant.NoAutoTransaction)]
    [InlineData(SaveVariant.OptimizedDapper | SaveVariant.NoAutoTransaction)]
    public async Task GivenSaveChangesAsync_WhenNoTransaction_ShouldThrowsException(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        await db.Context.AddAsync(ItemResolver(1));

        // Act
        Func<Task> result = () => db.SaveAsync(variant, null, 0);

        // Assert
        await result.Should().ThrowExactlyAsync<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenDifferentOperations_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 10);

        var toAdd = new object[] { ItemResolver(11), ItemResolver(12), ItemResolver(13) };
        NonRelatedEntity toEdit = data[3];
        toEdit.SomeNullableStringProperty = "new-prop";
        NonRelatedEntity toRemove = data[6];

        await db.Context.AddRangeAsync(toAdd);
        db.Context.Update(toEdit);
        db.Context.Remove(toRemove);

        // Act
        await db.SaveAsync(variant, null);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetryAsync();

        var properties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(12);

        properties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13);

        result[3].SomeNullableStringProperty.Should().BeEquivalentTo("new-prop");
    }

    [Theory]
    [InlineData(SaveVariant.Optimized | SaveVariant.NoAutoTransaction)]
    [InlineData(SaveVariant.OptimizedDapper | SaveVariant.NoAutoTransaction)]
    public void GivenSaveChanges_WhenNoTransaction_ShouldThrowsException(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        db.Context.Add(ItemResolver(1));

        // Act
        Action result = () => db.Save(variant, null, 0);

        // Assert
        result.Should().ThrowExactly<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenDifferentOperations_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 10);

        var toAdd = new object[] { ItemResolver(11), ItemResolver(12), ItemResolver(13) };
        NonRelatedEntity toEdit = data[3];
        toEdit.SomeNullableStringProperty = "new-prop";
        NonRelatedEntity toRemove = data[6];

        db.Context.AddRange(toAdd);
        db.Context.Update(toEdit);
        db.Context.Remove(toRemove);

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.SomeNonNullableIntProperty).ToArrayWithRetry();

        var properties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(12);

        properties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13);

        result[3].SomeNullableStringProperty.Should().BeEquivalentTo("new-prop");
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
