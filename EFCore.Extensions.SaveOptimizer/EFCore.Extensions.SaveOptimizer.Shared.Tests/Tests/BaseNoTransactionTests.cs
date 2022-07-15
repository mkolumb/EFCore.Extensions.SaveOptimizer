using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;

// ReSharper disable AccessToDisposedClosure

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract partial class BaseMiscTests
{
    [SkippableTheory]
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

    [SkippableTheory]
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
}
