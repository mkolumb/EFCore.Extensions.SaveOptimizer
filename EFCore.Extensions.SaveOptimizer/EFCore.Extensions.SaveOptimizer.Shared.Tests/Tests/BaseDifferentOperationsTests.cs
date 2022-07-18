using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseDifferentOperationsTests : BaseTests
{
    protected BaseDifferentOperationsTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenDifferentOperations_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeedAsync(db, variant, 10).ConfigureAwait(false);

        var toAdd = new object[] { ItemResolver(11), ItemResolver(12), ItemResolver(13) };
        NonRelatedEntity toEdit = data[3];
        toEdit.SomeNullableStringProperty = "new-prop";
        NonRelatedEntity toRemove = data[6];

        await db.Context.AddRangeAsync(toAdd).ConfigureAwait(false);
        db.Context.Update(toEdit);
        db.Context.Remove(toRemove);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity[] result =
            await db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var properties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(12);

        properties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13);

        result[3].SomeNullableStringProperty.Should().BeEquivalentTo("new-prop");
    }

    [SkippableTheory]
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
            db.Context.NonRelatedEntities
                .OrderBy(x => x.Indexer)
                .ToArrayWithRetry();

        var properties = result.Select(x => x.SomeNonNullableIntProperty).ToArray();

        // Assert
        result.Should().HaveCount(12);

        properties.Should()
            .ContainInOrder(0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13);

        result[3].SomeNullableStringProperty.Should().BeEquivalentTo("new-prop");
    }
}
