using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BasePrimaryKeyTests : BaseTests
{
    protected BasePrimaryKeyTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenInsertAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        AutoIncrementEntity[] result =
            await db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().ContainInOrder(1, 2, 3);

        properties.Should().ContainInOrder("x1", "x2", "x3");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenUpdateAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        data = await db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        foreach (AutoIncrementEntity item in data)
        {
            item.Some = $"a_{item.Some}_{item.Id}";
        }

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        AutoIncrementEntity[] result =
            await db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().ContainInOrder(1, 2, 3);

        properties.Should().ContainInOrder("a_x1_1", "a_x2_2", "a_x3_3");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenDeleteAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        data = await db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        db.Context.RemoveRange(data.Take(2));

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        AutoIncrementEntity[] result =
            await db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(1);

        keys.Should().ContainInOrder(3);

        properties.Should().ContainInOrder("x3");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenInsertAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        db.Context.AddRange(data as IEnumerable<object>);

        // Act
        db.Save(variant, null);

        AutoIncrementEntity[] result =
            db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetry();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().ContainInOrder(1, 2, 3);

        properties.Should().ContainInOrder("x1", "x2", "x3");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenUpdateAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        db.Context.AddRange(data as IEnumerable<object>);

        db.Save(variant, null);

        data = db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetry();

        foreach (AutoIncrementEntity item in data)
        {
            item.Some = $"a_{item.Some}_{item.Id}";
        }

        // Act
        db.Save(variant, null);

        AutoIncrementEntity[] result =
            db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetry();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().ContainInOrder(1, 2, 3);

        properties.Should().ContainInOrder("a_x1_1", "a_x2_2", "a_x3_3");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenDeleteAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        db.Context.AddRange(data as IEnumerable<object>);

        db.Save(variant, null);

        data = db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetry();

        db.Context.RemoveRange(data.Take(2));

        // Act
        db.Save(variant, null);

        AutoIncrementEntity[] result =
            db.Context.AutoIncrementEntities.OrderBy(x => x.Some).ToArrayWithRetry();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(1);

        keys.Should().ContainInOrder(3);

        properties.Should().ContainInOrder("x3");
    }
}
