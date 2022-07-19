using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseInsertTests : BaseTests
{
    protected BaseInsertTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(InsertData))]
    public async Task GivenSaveChangesAsync_WhenMultipleObjectsInserted_ShouldInsertData(SaveVariant variant,
        int? batchSize, int count)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        // Act
        for (var i = 0; i < count; i++)
        {
            await db.Context.AddAsync(ItemResolver(i)).ConfigureAwait(false);
        }

        await db.SaveAsync(variant, batchSize).ConfigureAwait(false);

        var result = await db.Context.NonRelatedEntities.CountAsync().ConfigureAwait(false);

        var properties = await db.Context.NonRelatedEntities
            .Select(x => x.NonNullableString)
            .Distinct()
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(count);

        properties.Should().HaveCount(count);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        var result = await db.Context.NonRelatedEntities.CountAsync().ConfigureAwait(false);

        // Assert
        result.Should().Be(0);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenOneObjectInserted_ShouldInsertData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity item = new()
        {
            ConcurrencyToken = new DateTimeOffset(2033, 11, 11, 2, 3, 4, 5, TimeSpan.Zero),
            NonNullableBoolean = true,
            NonNullableDateTime = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            NullableDateTime = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            NonNullableDecimal = 2.52M,
            NullableDecimal = 4.523435M,
            NonNullableInt = 1,
            NullableInt = 11,
            NonNullableString = "some-string",
            NullableString = "other-string"
        };

        // Act
        await db.Context.AddAsync(item).ConfigureAwait(false);

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity result = await db.Context.NonRelatedEntities.FirstAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.NonRelatedEntityId.Should().NotBeEmpty();
        result.ConcurrencyToken.Should().BeCloseTo(item.ConcurrencyToken.Value, 1.Seconds());
        result.NonNullableBoolean.Should().Be(item.NonNullableBoolean);
        result.NonNullableDateTime.Should()
            .BeCloseTo(item.NonNullableDateTime.Value, 1.Seconds());
        result.NullableDateTime.Should().BeCloseTo(item.NullableDateTime.Value, 1.Seconds());
        result.NonNullableDecimal.Should().Be(item.NonNullableDecimal);
        result.NullableDecimal.Should().Be(item.NullableDecimal);
        result.NonNullableInt.Should().Be(item.NonNullableInt);
        result.NullableInt.Should().Be(item.NullableInt);
        result.NonNullableString.Should().Be(item.NonNullableString);
        result.NullableString.Should().Be(item.NullableString);
    }

    [SkippableTheory]
    [MemberData(nameof(InsertData))]
    public void GivenSaveChanges_WhenMultipleObjectsInserted_ShouldInsertData(SaveVariant variant, int? batchSize,
        int count)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        // Act
        for (var i = 0; i < count; i++)
        {
            db.Context.Add(ItemResolver(i));
        }

        db.Save(variant, batchSize);

        var result = db.Context.NonRelatedEntities.Count();

        var properties = db.Context.NonRelatedEntities
            .Select(x => x.NonNullableString)
            .Distinct()
            .ToArrayWithRetry();

        // Assert
        result.Should().Be(count);

        properties.Should().HaveCount(count);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        // Act
        db.Save(variant, null);

        var result = db.Context.NonRelatedEntities.Count();

        // Assert
        result.Should().Be(0);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenOneObjectInserted_ShouldInsertData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity item = new()
        {
            ConcurrencyToken = new DateTimeOffset(2033, 11, 11, 2, 3, 4, 5, TimeSpan.Zero),
            NonNullableBoolean = true,
            NonNullableDateTime = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            NullableDateTime = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            NonNullableDecimal = 2.52M,
            NullableDecimal = 4.523435M,
            NonNullableInt = 1,
            NullableInt = 11,
            NonNullableString = "some-string",
            NullableString = "other-string"
        };

        // Act
        db.Context.Add(item);

        db.Save(variant, null);

        NonRelatedEntity result = db.Context.NonRelatedEntities.First();

        // Assert
        result.Should().NotBeNull();
        result.NonRelatedEntityId.Should().NotBeEmpty();
        result.ConcurrencyToken.Should().BeCloseTo(item.ConcurrencyToken.Value, 1.Seconds());
        result.NonNullableBoolean.Should().Be(item.NonNullableBoolean);
        result.NonNullableDateTime.Should()
            .BeCloseTo(item.NonNullableDateTime.Value, 1.Seconds());
        result.NullableDateTime.Should().BeCloseTo(item.NullableDateTime.Value, 1.Seconds());
        result.NonNullableDecimal.Should().Be(item.NonNullableDecimal);
        result.NullableDecimal.Should().Be(item.NullableDecimal);
        result.NonNullableInt.Should().Be(item.NonNullableInt);
        result.NullableInt.Should().Be(item.NullableInt);
        result.NonNullableString.Should().Be(item.NonNullableString);
        result.NullableString.Should().Be(item.NullableString);
    }
}
