using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseUpdateTests : BaseTests
{
    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => SharedTheoryData.BaseWriteTheoryData;

    protected BaseUpdateTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 10).ConfigureAwait(false);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            entity.SomeNonNullableIntProperty++;
            entity.SomeNonNullableIntProperty--;
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var newState = JsonConvert.SerializeObject(result);

        // Assert
        result.Should().HaveCount(10);

        newState.Should().BeEquivalentTo(state);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenOneObjectUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 3).ConfigureAwait(false);

        data[0].SomeNonNullableDecimalProperty = 191;

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(3);
        result[0].SomeNonNullableDecimalProperty.Should().Be(191);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[2].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenMultipleObjectsUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 15).ConfigureAwait(false);

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
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

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

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenMultipleObjectsUpdatedAndSomePropertiesAreTheSame_ShouldUpdateData(
        SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 15).ConfigureAwait(false);

        for (var i = 0; i < 5; i++)
        {
            data[i].SomeNullableIntProperty = i;
            data[i].SomeNonNullableIntProperty = i;
        }

        data[5].SomeNullableIntProperty = null;
        data[5].SomeNonNullableIntProperty = 0;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 != 0)
            {
                continue;
            }

            data[i].SomeNullableDecimalProperty = 191;
            data[i].SomeNonNullableDecimalProperty = 191;
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var nullableIntProperties = result.Select(x => x.SomeNullableIntProperty).ToArray();

        var nonNullableIntProperties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        var nullableDecimalProperties = result.Select(x => x.SomeNullableDecimalProperty).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.SomeNonNullableDecimalProperty).ToArray();

        // Assert
        result.Should().HaveCount(15);
        nullableIntProperties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, null, 11, 11, 11, 11, 11, 11, 11, 11, 11);
        nonNullableIntProperties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        nullableDecimalProperties.Should()
            .ContainInOrder(191, 4.523M, 4.523M, 191, 4.523M, 4.523M, 191, 4.523M, 4.523M, 191, 4.523M, 4.523M, 191,
                4.523M,
                4.523M);
        nonNullableDecimalProperties.Should()
            .ContainInOrder(191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M,
                2.52M);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 10);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            entity.SomeNonNullableIntProperty++;
            entity.SomeNonNullableIntProperty--;
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetry();

        var newState = JsonConvert.SerializeObject(result);

        // Assert
        result.Should().HaveCount(10);

        newState.Should().BeEquivalentTo(state);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenOneObjectUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 3);

        data[0].SomeNonNullableDecimalProperty = 191;

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(3);
        result[0].SomeNonNullableDecimalProperty.Should().Be(191);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[2].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenMultipleObjectsUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 15);

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
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetry();

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

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenMultipleObjectsUpdatedAndSomePropertiesAreTheSame_ShouldUpdateData(
        SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 15);

        for (var i = 0; i < 5; i++)
        {
            data[i].SomeNullableIntProperty = i;
            data[i].SomeNonNullableIntProperty = i;
        }

        data[5].SomeNullableIntProperty = null;
        data[5].SomeNonNullableIntProperty = 0;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 != 0)
            {
                continue;
            }

            data[i].SomeNullableDecimalProperty = 191;
            data[i].SomeNonNullableDecimalProperty = 191;
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetry();

        var nullableIntProperties = result.Select(x => x.SomeNullableIntProperty).ToArray();

        var nonNullableIntProperties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        var nullableDecimalProperties = result.Select(x => x.SomeNullableDecimalProperty).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.SomeNonNullableDecimalProperty).ToArray();

        // Assert
        result.Should().HaveCount(15);
        nullableIntProperties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, null, 11, 11, 11, 11, 11, 11, 11, 11, 11);
        nonNullableIntProperties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        nullableDecimalProperties.Should()
            .ContainInOrder(191, 4.523M, 4.523M, 191, 4.523M, 4.523M, 191, 4.523M, 4.523M, 191, 4.523M, 4.523M, 191,
                4.523M,
                4.523M);
        nonNullableDecimalProperties.Should()
            .ContainInOrder(191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M, 2.52M, 191, 2.52M,
                2.52M);
    }

    private static async Task<NonRelatedEntity[]> InitialSeedAsync(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await db.Context.AddAsync(ItemResolver(i)).ConfigureAwait(false);
        }

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        return await db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetryAsync()
            .ConfigureAwait(false);
    }

    private static NonRelatedEntity[] InitialSeed(DbContextWrapper db, SaveVariant variant, int count)
    {
        for (var i = 0; i < count; i++)
        {
            db.Context.Add(ItemResolver(i));
        }

        db.Save(variant, null);

        return db.Context.NonRelatedEntities.OrderBy(x => x.NonRelatedEntityId).ToArrayWithRetry();
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
