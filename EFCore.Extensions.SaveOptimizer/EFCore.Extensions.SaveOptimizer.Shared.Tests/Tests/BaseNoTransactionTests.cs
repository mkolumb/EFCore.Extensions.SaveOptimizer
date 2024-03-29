﻿using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

// ReSharper disable AccessToDisposedClosure

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseNoTransactionTests : BaseTests
{
    protected BaseNoTransactionTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [InlineData(SaveVariant.Optimized | SaveVariant.NoAutoTransaction)]
    [InlineData(SaveVariant.OptimizedDapper | SaveVariant.NoAutoTransaction)]
    public async Task GivenSaveChangesAsync_WhenNoTransaction_ShouldThrowsException(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        await db.Context.AddAsync(ItemResolver(1)).ConfigureAwait(false);

        // Act
        Func<Task> result = () => db.SaveAsync(variant, null, 0);

        // Assert
        await result.Should().ThrowExactlyAsync<ArgumentException>().ConfigureAwait(false);
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
