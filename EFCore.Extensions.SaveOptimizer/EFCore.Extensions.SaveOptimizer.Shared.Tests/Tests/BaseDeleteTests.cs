using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseDeleteTests : BaseTests
{
    protected BaseDeleteTests(ITestOutputHelper testOutputHelper,
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

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 10).ConfigureAwait(false);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            db.Context.NonRelatedEntities.Remove(entity);
            db.Context.NonRelatedEntities.Update(entity);
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
    public async Task GivenSaveChangesAsync_WhenOneObjectDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 3).ConfigureAwait(false);

        db.Context.NonRelatedEntities.Remove(data[0]);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(2);
        result[0].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenMultipleObjectsDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 15).ConfigureAwait(false);

        for (var i = 0; i < 5; i++)
        {
            db.Context.NonRelatedEntities.Remove(data[i]);
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var nonNullableIntProperties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(10);
        nonNullableIntProperties.Should().ContainInOrder(5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
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
            db.Context.NonRelatedEntities.Remove(entity);
            db.Context.NonRelatedEntities.Update(entity);
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
    public void GivenSaveChanges_WhenOneObjectDeleted_ShouldDeleteData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = InitialSeed(db, variant, 3);

        db.Context.NonRelatedEntities.Remove(data[0]);

        // Act
        db.Save(variant, null);

        NonRelatedEntity[] result =
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(2);
        result[0].SomeNonNullableDecimalProperty.Should().Be(2.52M);
        result[1].SomeNonNullableDecimalProperty.Should().Be(2.52M);
    }

    [SkippableTheory]
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
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

        var nonNullableIntProperties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(10);
        nonNullableIntProperties.Should().ContainInOrder(5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
    }
}
