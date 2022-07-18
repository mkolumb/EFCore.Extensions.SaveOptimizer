using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract class BaseValueConverterTests : BaseTests
{
    protected BaseValueConverterTests(ITestOutputHelper testOutputHelper,
        Func<ITestOutputHelper, EntityCollectionAttribute?, DbContextWrapper> contextWrapperResolver)
        : base(testOutputHelper, contextWrapperResolver)
    {
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenValueConverter_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ValueConverterEntity[] data =
        {
            new() { Id = Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"), SomeHalf = (Half)2.445 },
            new() { Id = Guid.Parse("b94c49be-6867-48d8-b001-8f540eb4c243"), SomeHalf = (Half)2.845 }
        };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        ValueConverterEntity[] result = await db.Context.ValueConverterEntities.OrderBy(x => x.Id)
            .ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"));
        result[0].SomeHalf.Should().Be((Half)2.445);

        result[1].Id.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-8f540eb4c243"));
        result[1].SomeHalf.Should().Be((Half)2.845);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenValueConverter_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        ValueConverterEntity[] data =
        {
            new() { Id = Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"), SomeHalf = (Half)2.445 },
            new() { Id = Guid.Parse("b94c49be-6867-48d8-b001-8f540eb4c243"), SomeHalf = (Half)2.845 }
        };

        db.Context.AddRange(data as IEnumerable<object>);

        // Act
        db.Save(variant, null);

        ValueConverterEntity[] result = db.Context.ValueConverterEntities.OrderBy(x => x.Id).ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"));
        result[0].SomeHalf.Should().Be((Half)2.445);

        result[1].Id.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-8f540eb4c243"));
        result[1].SomeHalf.Should().Be((Half)2.845);
    }
}
