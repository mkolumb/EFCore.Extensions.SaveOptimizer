using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public abstract class BaseUpdateTests
{
    public Func<DbContextWrapper> ContextWrapperResolver { get; }

    protected BaseUpdateTests(Func<DbContextWrapper> contextWrapperResolver) =>
        ContextWrapperResolver = contextWrapperResolver;

    [Theory]
    [InlineData(SaveVariant.EfCore | SaveVariant.Recreate)]
    [InlineData(SaveVariant.EfCore | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate)]
    [InlineData(SaveVariant.Optimized | SaveVariant.Recreate | SaveVariant.WithTransaction)]
    public async Task GivenSaveChanges_WhenNoChanges_ShouldDoNothing(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        NonRelatedEntity[] data = await InitialSeed(db, variant);

        var state = JsonConvert.SerializeObject(data);

        foreach (NonRelatedEntity entity in data)
        {
            entity.SomeNonNullableIntProperty++;
            entity.SomeNonNullableIntProperty--;
        }

        // Act
        await db.Save(variant);

        NonRelatedEntity[] result = await db.Context.NonRelatedEntities.ToArrayAsync();

        var newState = JsonConvert.SerializeObject(result);

        // Assert
        result.Should().HaveCount(10);

        newState.Should().BeEquivalentTo(state);
    }

    private static async Task<NonRelatedEntity[]> InitialSeed(DbContextWrapper db, SaveVariant variant)
    {
        for (var i = 0; i < 10; i++)
        {
            await db.Context.AddAsync(ItemResolver(i));
        }

        await db.Save(variant);

        return await db.Context.NonRelatedEntities.ToArrayAsync();
    }

    private static NonRelatedEntity ItemResolver(int i) =>
        new()
        {
            ConcurrencyToken = DateTimeOffset.UtcNow,
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
