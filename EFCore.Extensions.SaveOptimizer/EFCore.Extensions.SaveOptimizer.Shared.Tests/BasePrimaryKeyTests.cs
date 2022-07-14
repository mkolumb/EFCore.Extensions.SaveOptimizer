using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract partial class BaseMiscTests
{
    [Theory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenAutoIncrementPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        var data = new object[]
        {
            new AutoIncrementPrimaryKeyEntity { Some = "x1" }, new AutoIncrementPrimaryKeyEntity { Some = "x2" },
            new AutoIncrementPrimaryKeyEntity { Some = "x3" }
        };

        await db.Context.AddRangeAsync(data);

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
}
