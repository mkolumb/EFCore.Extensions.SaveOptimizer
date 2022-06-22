using EFCore.Extensions.SaveOptimizer.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract class BaseInsertTests
{
    public Func<DbContextWrapper> ContextWrapperResolver { get; }

    protected BaseInsertTests(Func<DbContextWrapper> contextWrapperResolver) =>
        ContextWrapperResolver = contextWrapperResolver;

    [Theory]
    [InlineData(SaveVariant.Normal | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Normal | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    public async Task GivenSaveChanges_WhenMultipleObjectsInserted_ShouldInsertData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity ItemResolver()
        {
            return new NonRelatedEntity
            {
                ConcurrencyToken = DateTime.Now,
                SomeNonNullableBooleanProperty = true,
                SomeNonNullableDateTimeProperty = new DateTime(2010, 10, 10, 1, 2, 3),
                SomeNullableDateTimeProperty = new DateTime(2012, 11, 11, 1, 2, 3),
                SomeNonNullableDecimalProperty = 2.52M,
                SomeNullableDecimalProperty = 4.523M,
                SomeNonNullableIntProperty = 1,
                SomeNullableIntProperty = 11,
                SomeNonNullableStringProperty = "some-string",
                SomeNullableStringProperty = "other-string"
            };
        }

        // Act
        for (var i = 0; i < 10; i++)
        {
            await db.Context.AddAsync(ItemResolver());
        }

        await db.Save(variant);

        var result = await db.Context.NonRelatedEntities.CountAsync();

        // Assert
        result.Should().Be(10);
    }

    [Theory]
    [InlineData(SaveVariant.Normal | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Normal | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    public async Task GivenSaveChanges_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        // Act
        await db.Save(variant);

        var result = await db.Context.NonRelatedEntities.CountAsync();

        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(SaveVariant.Normal | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Normal | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    public async Task GivenSaveChanges_WhenOneObjectInserted_ShouldInsertData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity item = new()
        {
            ConcurrencyToken = DateTime.Now,
            SomeNonNullableBooleanProperty = true,
            SomeNonNullableDateTimeProperty = new DateTime(2010, 10, 10, 1, 2, 3),
            SomeNullableDateTimeProperty = new DateTime(2012, 11, 11, 1, 2, 3),
            SomeNonNullableDecimalProperty = 2.52M,
            SomeNullableDecimalProperty = 4.523M,
            SomeNonNullableIntProperty = 1,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = "some-string",
            SomeNullableStringProperty = "other-string"
        };

        // Act
        await db.Context.AddAsync(item);

        await db.Save(variant);

        NonRelatedEntity result = await db.Context.NonRelatedEntities.FirstAsync();

        // Assert
        result.Should().NotBeNull();
        result.NonRelatedEntityId.Should().NotBeEmpty();
        result.ConcurrencyToken.Should().Be(item.ConcurrencyToken);
        result.SomeNonNullableBooleanProperty.Should().Be(item.SomeNonNullableBooleanProperty);
        result.SomeNonNullableDateTimeProperty.Should().Be(item.SomeNonNullableDateTimeProperty);
        result.SomeNullableDateTimeProperty.Should().Be(item.SomeNullableDateTimeProperty);
        result.SomeNonNullableDecimalProperty.Should().Be(item.SomeNonNullableDecimalProperty);
        result.SomeNullableDecimalProperty.Should().Be(item.SomeNullableDecimalProperty);
        result.SomeNonNullableIntProperty.Should().Be(item.SomeNonNullableIntProperty);
        result.SomeNullableIntProperty.Should().Be(item.SomeNullableIntProperty);
        result.SomeNonNullableStringProperty.Should().Be(item.SomeNonNullableStringProperty);
        result.SomeNullableStringProperty.Should().Be(item.SomeNullableStringProperty);
    }
}
