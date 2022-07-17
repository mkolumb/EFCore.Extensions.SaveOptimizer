using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseInsertTests : BaseTests
{
    public static IEnumerable<IEnumerable<object?>> InsertData => SharedTheoryData.InsertTheoryData;

    public static IEnumerable<IEnumerable<object?>> BaseWriteTheoryData => SharedTheoryData.BaseWriteTheoryData;

    protected BaseInsertTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, DbContextWrapper> contextWrapperResolver)
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
            .Select(x => x.SomeNonNullableStringProperty)
            .Distinct()
            .ToArrayWithRetryAsync().ConfigureAwait(false);

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
            SomeNonNullableBooleanProperty = true,
            SomeNonNullableDateTimeProperty = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNullableDateTimeProperty = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNonNullableDecimalProperty = 2.52M,
            SomeNullableDecimalProperty = 4.523435M,
            SomeNonNullableIntProperty = 1,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = "some-string",
            SomeNullableStringProperty = "other-string"
        };

        // Act
        await db.Context.AddAsync(item).ConfigureAwait(false);

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        NonRelatedEntity result = await db.Context.NonRelatedEntities.FirstAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.NonRelatedEntityId.Should().NotBeEmpty();
        result.ConcurrencyToken.Should().BeCloseTo(item.ConcurrencyToken.Value, 1.Seconds());
        result.SomeNonNullableBooleanProperty.Should().Be(item.SomeNonNullableBooleanProperty);
        result.SomeNonNullableDateTimeProperty.Should()
            .BeCloseTo(item.SomeNonNullableDateTimeProperty.Value, 1.Seconds());
        result.SomeNullableDateTimeProperty.Should().BeCloseTo(item.SomeNullableDateTimeProperty.Value, 1.Seconds());
        result.SomeNonNullableDecimalProperty.Should().Be(item.SomeNonNullableDecimalProperty);
        result.SomeNullableDecimalProperty.Should().Be(item.SomeNullableDecimalProperty);
        result.SomeNonNullableIntProperty.Should().Be(item.SomeNonNullableIntProperty);
        result.SomeNullableIntProperty.Should().Be(item.SomeNullableIntProperty);
        result.SomeNonNullableStringProperty.Should().Be(item.SomeNonNullableStringProperty);
        result.SomeNullableStringProperty.Should().Be(item.SomeNullableStringProperty);
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
            .Select(x => x.SomeNonNullableStringProperty)
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
            SomeNonNullableBooleanProperty = true,
            SomeNonNullableDateTimeProperty = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNullableDateTimeProperty = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNonNullableDecimalProperty = 2.52M,
            SomeNullableDecimalProperty = 4.523435M,
            SomeNonNullableIntProperty = 1,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = "some-string",
            SomeNullableStringProperty = "other-string"
        };

        // Act
        db.Context.Add(item);

        db.Save(variant, null);

        NonRelatedEntity result = db.Context.NonRelatedEntities.First();

        // Assert
        result.Should().NotBeNull();
        result.NonRelatedEntityId.Should().NotBeEmpty();
        result.ConcurrencyToken.Should().BeCloseTo(item.ConcurrencyToken.Value, 1.Seconds());
        result.SomeNonNullableBooleanProperty.Should().Be(item.SomeNonNullableBooleanProperty);
        result.SomeNonNullableDateTimeProperty.Should()
            .BeCloseTo(item.SomeNonNullableDateTimeProperty.Value, 1.Seconds());
        result.SomeNullableDateTimeProperty.Should().BeCloseTo(item.SomeNullableDateTimeProperty.Value, 1.Seconds());
        result.SomeNonNullableDecimalProperty.Should().Be(item.SomeNonNullableDecimalProperty);
        result.SomeNullableDecimalProperty.Should().Be(item.SomeNullableDecimalProperty);
        result.SomeNonNullableIntProperty.Should().Be(item.SomeNonNullableIntProperty);
        result.SomeNullableIntProperty.Should().Be(item.SomeNullableIntProperty);
        result.SomeNonNullableStringProperty.Should().Be(item.SomeNonNullableStringProperty);
        result.SomeNullableStringProperty.Should().Be(item.SomeNullableStringProperty);
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
