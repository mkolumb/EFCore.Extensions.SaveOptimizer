using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract partial class BaseMiscTests
{
    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenInsertAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementPrimaryKeyEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        await db.Context.AddRangeAsync(data as IEnumerable<object>);

        // Act
        await db.SaveAsync(variant, null);

        AutoIncrementPrimaryKeyEntity[] result =
            await db.Context.AutoIncrementPrimaryKeyEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().ContainInOrder(1, 2, 3);

        properties.Should().ContainInOrder("x1", "x2", "x3");
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenUpdateAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementPrimaryKeyEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        await db.Context.AddRangeAsync(data as IEnumerable<object>);

        await db.SaveAsync(variant, null);

        data = await db.Context.AutoIncrementPrimaryKeyEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync();

        foreach (AutoIncrementPrimaryKeyEntity item in data)
        {
            item.Some = $"a_{item.Some}_{item.Id}";
        }

        // Act
        await db.SaveAsync(variant, null);

        AutoIncrementPrimaryKeyEntity[] result =
            await db.Context.AutoIncrementPrimaryKeyEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().ContainInOrder(1, 2, 3);

        properties.Should().ContainInOrder("a_x1_1", "a_x2_2", "a_x3_3");
    }

    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenDeleteAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        AutoIncrementPrimaryKeyEntity[] data = { new() { Some = "x1" }, new() { Some = "x2" }, new() { Some = "x3" } };

        await db.Context.AddRangeAsync(data as IEnumerable<object>);

        await db.SaveAsync(variant, null);

        data = await db.Context.AutoIncrementPrimaryKeyEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync();

        db.Context.RemoveRange(data.Take(2));

        // Act
        await db.SaveAsync(variant, null);

        AutoIncrementPrimaryKeyEntity[] result =
            await db.Context.AutoIncrementPrimaryKeyEntities.OrderBy(x => x.Some).ToArrayWithRetryAsync();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(1);

        keys.Should().ContainInOrder(3);

        properties.Should().ContainInOrder("x3");
    }
}
