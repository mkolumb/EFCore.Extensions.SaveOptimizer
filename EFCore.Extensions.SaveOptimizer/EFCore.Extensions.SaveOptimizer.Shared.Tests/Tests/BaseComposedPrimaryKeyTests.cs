using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseComposedPrimaryKeyTests : BaseTests
{
    protected BaseComposedPrimaryKeyTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenInsertComposedPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ComposedEntity[] data =
        {
            new() { PrimaryFirst = 1, PrimarySecond = 1, Some = "some-1" },
            new() { PrimaryFirst = 1, PrimarySecond = 2, Some = "some-2" },
            new() { PrimaryFirst = 2, PrimarySecond = 1, Some = "some-3" },
            new() { PrimaryFirst = 2, PrimarySecond = 2, Some = "some-4" }
        };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        ComposedEntity[] result = await db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(4);

        result[0].PrimaryFirst.Should().Be(1);
        result[0].PrimarySecond.Should().Be(1);
        result[0].Some.Should().Be("some-1");

        result[1].PrimaryFirst.Should().Be(1);
        result[1].PrimarySecond.Should().Be(2);
        result[1].Some.Should().Be("some-2");

        result[2].PrimaryFirst.Should().Be(2);
        result[2].PrimarySecond.Should().Be(1);
        result[2].Some.Should().Be("some-3");

        result[3].PrimaryFirst.Should().Be(2);
        result[3].PrimarySecond.Should().Be(2);
        result[3].Some.Should().Be("some-4");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenUpdateComposedPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ComposedEntity[] data =
        {
            new() { PrimaryFirst = 1, PrimarySecond = 1 }, new() { PrimaryFirst = 1, PrimarySecond = 2 },
            new() { PrimaryFirst = 2, PrimarySecond = 1 }, new() { PrimaryFirst = 2, PrimarySecond = 2 }
        };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        data = await db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        data[0].Some = "some-1";
        data[1].Some = "some-2";
        data[2].Some = "some-3";
        data[3].Some = "some-4";

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        ComposedEntity[] result = await db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(4);

        result[0].PrimaryFirst.Should().Be(1);
        result[0].PrimarySecond.Should().Be(1);
        result[0].Some.Should().Be("some-1");

        result[1].PrimaryFirst.Should().Be(1);
        result[1].PrimarySecond.Should().Be(2);
        result[1].Some.Should().Be("some-2");

        result[2].PrimaryFirst.Should().Be(2);
        result[2].PrimarySecond.Should().Be(1);
        result[2].Some.Should().Be("some-3");

        result[3].PrimaryFirst.Should().Be(2);
        result[3].PrimarySecond.Should().Be(2);
        result[3].Some.Should().Be("some-4");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenDeleteComposedPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ComposedEntity[] data =
        {
            new() { PrimaryFirst = 1, PrimarySecond = 1, Some = "some-1" },
            new() { PrimaryFirst = 1, PrimarySecond = 2, Some = "some-2" },
            new() { PrimaryFirst = 2, PrimarySecond = 1, Some = "some-3" },
            new() { PrimaryFirst = 2, PrimarySecond = 2, Some = "some-4" }
        };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        await db.SaveAsync(variant, null).ConfigureAwait(false);

        data = await db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        db.Context.RemoveRange(data[0], data[2]);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        ComposedEntity[] result = await db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(2);

        result[0].PrimaryFirst.Should().Be(1);
        result[0].PrimarySecond.Should().Be(2);
        result[0].Some.Should().Be("some-2");

        result[1].PrimaryFirst.Should().Be(2);
        result[1].PrimarySecond.Should().Be(2);
        result[1].Some.Should().Be("some-4");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenInsertComposedPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ComposedEntity[] data =
        {
            new() { PrimaryFirst = 1, PrimarySecond = 1, Some = "some-1" },
            new() { PrimaryFirst = 1, PrimarySecond = 2, Some = "some-2" },
            new() { PrimaryFirst = 2, PrimarySecond = 1, Some = "some-3" },
            new() { PrimaryFirst = 2, PrimarySecond = 2, Some = "some-4" }
        };

        db.Context.AddRange(data as IEnumerable<object>);

        // Act
        db.Save(variant, null);

        ComposedEntity[] result = db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(4);

        result[0].PrimaryFirst.Should().Be(1);
        result[0].PrimarySecond.Should().Be(1);
        result[0].Some.Should().Be("some-1");

        result[1].PrimaryFirst.Should().Be(1);
        result[1].PrimarySecond.Should().Be(2);
        result[1].Some.Should().Be("some-2");

        result[2].PrimaryFirst.Should().Be(2);
        result[2].PrimarySecond.Should().Be(1);
        result[2].Some.Should().Be("some-3");

        result[3].PrimaryFirst.Should().Be(2);
        result[3].PrimarySecond.Should().Be(2);
        result[3].Some.Should().Be("some-4");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenUpdateComposedPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ComposedEntity[] data =
        {
            new() { PrimaryFirst = 1, PrimarySecond = 1 }, new() { PrimaryFirst = 1, PrimarySecond = 2 },
            new() { PrimaryFirst = 2, PrimarySecond = 1 }, new() { PrimaryFirst = 2, PrimarySecond = 2 }
        };

        db.Context.AddRange(data as IEnumerable<object>);

        db.Save(variant, null);

        data = db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetry();

        data[0].Some = "some-1";
        data[1].Some = "some-2";
        data[2].Some = "some-3";
        data[3].Some = "some-4";

        // Act
        db.Save(variant, null);

        ComposedEntity[] result = db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(4);

        result[0].PrimaryFirst.Should().Be(1);
        result[0].PrimarySecond.Should().Be(1);
        result[0].Some.Should().Be("some-1");

        result[1].PrimaryFirst.Should().Be(1);
        result[1].PrimarySecond.Should().Be(2);
        result[1].Some.Should().Be("some-2");

        result[2].PrimaryFirst.Should().Be(2);
        result[2].PrimarySecond.Should().Be(1);
        result[2].Some.Should().Be("some-3");

        result[3].PrimaryFirst.Should().Be(2);
        result[3].PrimarySecond.Should().Be(2);
        result[3].Some.Should().Be("some-4");
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenDeleteComposedPrimaryKey_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ComposedEntity[] data =
        {
            new() { PrimaryFirst = 1, PrimarySecond = 1, Some = "some-1" },
            new() { PrimaryFirst = 1, PrimarySecond = 2, Some = "some-2" },
            new() { PrimaryFirst = 2, PrimarySecond = 1, Some = "some-3" },
            new() { PrimaryFirst = 2, PrimarySecond = 2, Some = "some-4" }
        };

        db.Context.AddRange(data as IEnumerable<object>);

        db.Save(variant, null);

        data = db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetry();

        db.Context.RemoveRange(data[0], data[2]);

        // Act
        db.Save(variant, null);

        ComposedEntity[] result = db.Context.ComposedEntities
            .OrderBy(x => x.PrimaryFirst)
            .ThenBy(x => x.PrimarySecond)
            .ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(2);

        result[0].PrimaryFirst.Should().Be(1);
        result[0].PrimarySecond.Should().Be(2);
        result[0].Some.Should().Be("some-2");

        result[1].PrimaryFirst.Should().Be(2);
        result[1].PrimarySecond.Should().Be(2);
        result[1].Some.Should().Be("some-4");
    }
}
