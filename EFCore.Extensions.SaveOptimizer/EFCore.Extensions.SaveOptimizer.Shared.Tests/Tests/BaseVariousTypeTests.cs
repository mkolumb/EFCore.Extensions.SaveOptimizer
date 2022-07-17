using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Model.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract partial class BaseMiscTests
{
    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenVariousType_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        VariousTypeEntity[] data =
        {
            new()
            {
                Id = 12,
                SomeBool = true,
                SomeByte = 176,
                SomeDecimal = 15.781235M,
                SomeDouble = 15.12,
                SomeFloat = 4.5f,
                SomeInt = 1232327,
                SomeLong = 14312353426357,
                SomeShort = 12322,
                SomeSignedByte = 114,
                SomeUnsignedInt = 1232327,
                SomeUnsignedLong = 14312353426357,
                SomeUnsignedShort = 12322,
                SomeTimeSpan = TimeSpan.FromMilliseconds(124566),
                SomeEnum = ExampleEnum.Val1,
                SomeString = "Some string",
                SomeDateTime = new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc),
                SomeDateTimeOffset = new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero),
                SomeGuid = Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243")
            },
            new()
            {
                Id = 72,
                SomeBool = true,
                SomeByte = 176,
                SomeDecimal = -15.781235M,
                SomeDouble = -15.12,
                SomeFloat = -4.5f,
                SomeInt = -1232327,
                SomeLong = -14312353426357,
                SomeShort = -12322,
                SomeSignedByte = -114,
                SomeUnsignedInt = 1232327,
                SomeUnsignedLong = 14312353426357,
                SomeUnsignedShort = 12322,
                SomeTimeSpan = TimeSpan.FromMilliseconds(124566),
                SomeEnum = ExampleEnum.Val4,
                SomeString = "Some string",
                SomeDateTime = new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc),
                SomeDateTimeOffset = new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero),
                SomeGuid = Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243")
            },
            new() { Id = 143 }
        };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        // Act
        await db.SaveAsync(variant, null).ConfigureAwait(false);

        VariousTypeEntity[] result = await db.Context.VariousTypeEntities.OrderBy(x => x.Id).ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        // Assert
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(12);
        result[0].SomeBool.Should().Be(true);
        result[0].SomeByte.Should().Be(176);
        result[0].SomeDecimal.Should().Be(15.781235M);
        result[0].SomeDouble.Should().Be(15.12);
        result[0].SomeFloat.Should().Be(4.5f);
        result[0].SomeInt.Should().Be(1232327);
        result[0].SomeLong.Should().Be(14312353426357);
        result[0].SomeShort.Should().Be(12322);
        result[0].SomeSignedByte.Should().Be(114);
        result[0].SomeUnsignedInt.Should().Be(1232327);
        result[0].SomeUnsignedLong.Should().Be(14312353426357);
        result[0].SomeUnsignedShort.Should().Be(12322);
        result[0].SomeTimeSpan.Should().Be(TimeSpan.FromMilliseconds(124566));
        result[0].SomeEnum.Should().Be(ExampleEnum.Val1);
        result[0].SomeString.Should().Be("Some string");
        result[0].SomeDateTime.Should().Be(new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc));
        result[0].SomeDateTimeOffset.Should().Be(new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero));
        result[0].SomeGuid.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"));

        result[1].Id.Should().Be(72);
        result[1].SomeBool.Should().Be(true);
        result[1].SomeByte.Should().Be(176);
        result[1].SomeDecimal.Should().Be(-15.781235M);
        result[1].SomeDouble.Should().Be(-15.12);
        result[1].SomeFloat.Should().Be(-4.5f);
        result[1].SomeInt.Should().Be(-1232327);
        result[1].SomeLong.Should().Be(-14312353426357);
        result[1].SomeShort.Should().Be(-12322);
        result[1].SomeSignedByte.Should().Be(-114);
        result[1].SomeUnsignedInt.Should().Be(1232327);
        result[1].SomeUnsignedLong.Should().Be(14312353426357);
        result[1].SomeUnsignedShort.Should().Be(12322);
        result[1].SomeTimeSpan.Should().Be(TimeSpan.FromMilliseconds(124566));
        result[1].SomeEnum.Should().Be(ExampleEnum.Val4);
        result[1].SomeString.Should().Be("Some string");
        result[1].SomeDateTime.Should().Be(new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc));
        result[1].SomeDateTimeOffset.Should().Be(new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero));
        result[1].SomeGuid.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"));

        result[2].Id.Should().Be(143);
        result[2].SomeBool.Should().BeNull();
        result[2].SomeByte.Should().BeNull();
        result[2].SomeDecimal.Should().BeNull();
        result[2].SomeDouble.Should().BeNull();
        result[2].SomeFloat.Should().BeNull();
        result[2].SomeInt.Should().BeNull();
        result[2].SomeLong.Should().BeNull();
        result[2].SomeShort.Should().BeNull();
        result[2].SomeSignedByte.Should().BeNull();
        result[2].SomeUnsignedInt.Should().BeNull();
        result[2].SomeUnsignedLong.Should().BeNull();
        result[2].SomeUnsignedShort.Should().BeNull();
        result[2].SomeTimeSpan.Should().BeNull();
        result[2].SomeEnum.Should().BeNull();
        result[2].SomeString.Should().BeNull();
        result[2].SomeDateTime.Should().BeNull();
        result[2].SomeDateTimeOffset.Should().BeNull();
        result[2].SomeGuid.Should().BeNull();
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenVariousType_ShouldStoreData(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        VariousTypeEntity[] data =
        {
            new()
            {
                Id = 12,
                SomeBool = true,
                SomeByte = 176,
                SomeDecimal = 15.781235M,
                SomeDouble = 15.12,
                SomeFloat = 4.5f,
                SomeInt = 1232327,
                SomeLong = 14312353426357,
                SomeShort = 12322,
                SomeSignedByte = 114,
                SomeUnsignedInt = 1232327,
                SomeUnsignedLong = 14312353426357,
                SomeUnsignedShort = 12322,
                SomeTimeSpan = TimeSpan.FromMilliseconds(124566),
                SomeEnum = ExampleEnum.Val7,
                SomeString = "Some string",
                SomeDateTime = new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc),
                SomeDateTimeOffset = new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero),
                SomeGuid = Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243")
            },
            new()
            {
                Id = 72,
                SomeBool = true,
                SomeByte = 176,
                SomeDecimal = -15.781235M,
                SomeDouble = -15.12,
                SomeFloat = -4.5f,
                SomeInt = -1232327,
                SomeLong = -14312353426357,
                SomeShort = -12322,
                SomeSignedByte = -114,
                SomeUnsignedInt = 1232327,
                SomeUnsignedLong = 14312353426357,
                SomeUnsignedShort = 12322,
                SomeTimeSpan = TimeSpan.FromMilliseconds(124566),
                SomeEnum = ExampleEnum.Val4,
                SomeString = "Some string",
                SomeDateTime = new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc),
                SomeDateTimeOffset = new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero),
                SomeGuid = Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243")
            },
            new() { Id = 143 }
        };

        db.Context.AddRange(data as IEnumerable<object>);

        // Act
        db.Save(variant, null);

        VariousTypeEntity[] result = db.Context.VariousTypeEntities.OrderBy(x => x.Id).ToArrayWithRetry();

        // Assert
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(12);
        result[0].SomeBool.Should().Be(true);
        result[0].SomeByte.Should().Be(176);
        result[0].SomeDecimal.Should().Be(15.781235M);
        result[0].SomeDouble.Should().Be(15.12);
        result[0].SomeFloat.Should().Be(4.5f);
        result[0].SomeInt.Should().Be(1232327);
        result[0].SomeLong.Should().Be(14312353426357);
        result[0].SomeShort.Should().Be(12322);
        result[0].SomeSignedByte.Should().Be(114);
        result[0].SomeUnsignedInt.Should().Be(1232327);
        result[0].SomeUnsignedLong.Should().Be(14312353426357);
        result[0].SomeUnsignedShort.Should().Be(12322);
        result[0].SomeTimeSpan.Should().Be(TimeSpan.FromMilliseconds(124566));
        result[0].SomeEnum.Should().Be(ExampleEnum.Val7);
        result[0].SomeString.Should().Be("Some string");
        result[0].SomeDateTime.Should().Be(new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc));
        result[0].SomeDateTimeOffset.Should().Be(new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero));
        result[0].SomeGuid.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"));

        result[1].Id.Should().Be(72);
        result[1].SomeBool.Should().Be(true);
        result[1].SomeByte.Should().Be(176);
        result[1].SomeDecimal.Should().Be(-15.781235M);
        result[1].SomeDouble.Should().Be(-15.12);
        result[1].SomeFloat.Should().Be(-4.5f);
        result[1].SomeInt.Should().Be(-1232327);
        result[1].SomeLong.Should().Be(-14312353426357);
        result[1].SomeShort.Should().Be(-12322);
        result[1].SomeSignedByte.Should().Be(-114);
        result[1].SomeUnsignedInt.Should().Be(1232327);
        result[1].SomeUnsignedLong.Should().Be(14312353426357);
        result[1].SomeUnsignedShort.Should().Be(12322);
        result[1].SomeTimeSpan.Should().Be(TimeSpan.FromMilliseconds(124566));
        result[1].SomeEnum.Should().Be(ExampleEnum.Val4);
        result[1].SomeString.Should().Be("Some string");
        result[1].SomeDateTime.Should().Be(new DateTime(2011, 10, 27, 14, 17, 18, 211, DateTimeKind.Utc));
        result[1].SomeDateTimeOffset.Should().Be(new DateTimeOffset(2011, 10, 27, 14, 17, 18, 211, TimeSpan.Zero));
        result[1].SomeGuid.Should().Be(Guid.Parse("b94c49be-6867-48d8-b001-7f540eb4c243"));

        result[2].Id.Should().Be(143);
        result[2].SomeBool.Should().BeNull();
        result[2].SomeByte.Should().BeNull();
        result[2].SomeDecimal.Should().BeNull();
        result[2].SomeDouble.Should().BeNull();
        result[2].SomeFloat.Should().BeNull();
        result[2].SomeInt.Should().BeNull();
        result[2].SomeLong.Should().BeNull();
        result[2].SomeShort.Should().BeNull();
        result[2].SomeSignedByte.Should().BeNull();
        result[2].SomeUnsignedInt.Should().BeNull();
        result[2].SomeUnsignedLong.Should().BeNull();
        result[2].SomeUnsignedShort.Should().BeNull();
        result[2].SomeTimeSpan.Should().BeNull();
        result[2].SomeEnum.Should().BeNull();
        result[2].SomeString.Should().BeNull();
        result[2].SomeDateTime.Should().BeNull();
        result[2].SomeDateTimeOffset.Should().BeNull();
        result[2].SomeGuid.Should().BeNull();
    }
}
