using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseUpdateTests : BaseTests
{
    protected BaseUpdateTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 10, Setter).ConfigureAwait(false);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            entity.NonNullableInt++;
            entity.NonNullableInt--;
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
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

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 3, Setter).ConfigureAwait(false);

        data[0].NonNullableDecimal = 191;

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(3);
        result[0].NonNullableDecimal.Should().Be(191);
        result[1].NonNullableDecimal.Should().Be(2.52M);
        result[2].NonNullableDecimal.Should().Be(2.52M);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenMultipleObjectsUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 15, Setter).ConfigureAwait(false);

        for (var i = 0; i < 5; i++)
        {
            data[i].NullableInt = i;
        }

        data[5].NullableInt = null;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 == 0)
            {
                data[i].NonNullableDecimal = 191;
            }
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var nullableIntProperties = result.Select(x => x.NullableInt).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.NonNullableDecimal).ToArray();

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

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 15, Setter).ConfigureAwait(false);

        for (var i = 0; i < 5; i++)
        {
            data[i].NullableInt = i;
            data[i].NonNullableInt = i;
        }

        data[5].NullableInt = null;
        data[5].NonNullableInt = 0;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 != 0)
            {
                continue;
            }

            data[i].NullableDecimal = 191;
            data[i].NonNullableDecimal = 191;
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var nullableIntProperties = result.Select(x => x.NullableInt).ToArray();

        var nonNullableIntProperties = result.Select(x => x.NonNullableInt).ToArray();

        var nullableDecimalProperties = result.Select(x => x.NullableDecimal).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.NonNullableDecimal).ToArray();

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

        NonRelatedEntity[] data = InitialSeed(db, variant, 10, Setter);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            entity.NonNullableInt++;
            entity.NonNullableInt--;
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

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

        NonRelatedEntity[] data = InitialSeed(db, variant, 3, Setter);

        data[0].NonNullableDecimal = 191;

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(3);
        result[0].NonNullableDecimal.Should().Be(191);
        result[1].NonNullableDecimal.Should().Be(2.52M);
        result[2].NonNullableDecimal.Should().Be(2.52M);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenMultipleObjectsUpdated_ShouldUpdateData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 15, Setter);

        for (var i = 0; i < 5; i++)
        {
            data[i].NullableInt = i;
        }

        data[5].NullableInt = null;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 == 0)
            {
                data[i].NonNullableDecimal = 191;
            }
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

        var nullableIntProperties = result.Select(x => x.NullableInt).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.NonNullableDecimal).ToArray();

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

        NonRelatedEntity[] data = InitialSeed(db, variant, 15, Setter);

        for (var i = 0; i < 5; i++)
        {
            data[i].NullableInt = i;
            data[i].NonNullableInt = i;
        }

        data[5].NullableInt = null;
        data[5].NonNullableInt = 0;

        for (var i = 0; i < data.Length; i++)
        {
            if (i % 3 != 0)
            {
                continue;
            }

            data[i].NullableDecimal = 191;
            data[i].NonNullableDecimal = 191;
        }

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

        var nullableIntProperties = result.Select(x => x.NullableInt).ToArray();

        var nonNullableIntProperties = result.Select(x => x.NonNullableInt).ToArray();

        var nullableDecimalProperties = result.Select(x => x.NullableDecimal).ToArray();

        var nonNullableDecimalProperties = result.Select(x => x.NonNullableDecimal).ToArray();

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

    private static void Setter(NonRelatedEntity entity) => entity.NonNullableInt = 1;
}
