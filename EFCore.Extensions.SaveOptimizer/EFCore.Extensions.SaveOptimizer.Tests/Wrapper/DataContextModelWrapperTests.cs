using EFCore.Extensions.SaveOptimizer.Tests.TestContext;
using EFCore.Extensions.SaveOptimizer.Tests.TestContext.Logs;
using EFCore.Extensions.SaveOptimizer.Tests.TestContext.Models;
using EFCore.Extensions.SaveOptimizer.Wrappers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Tests.Wrapper;

public class DataContextModelWrapperTests
{
    private readonly DataContextModelWrapper<TestDataContext> _sut;

    public DataContextModelWrapperTests()
    {
        DbContextOptionsBuilder<TestDataContext> options =
            new DbContextOptionsBuilder<TestDataContext>().UseInMemoryDatabase("in_memory_db")
                .UseSnakeCaseNamingConvention();
        TestDataContext context = new(options.Options);
        _sut = new DataContextModelWrapper<TestDataContext>(() => context);
    }

    [Fact]
    public void GivenGetColumn_ShouldReturnsProperColumnEveryTime()
    {
        // Arrange / Act
        var results = new[]
        {
            _sut.GetColumn<FirstLevelEntity>(nameof(FirstLevelEntity.FirstIntNullable)),
            _sut.GetColumn<FirstLevelEntity>(nameof(FirstLevelEntity.UpdatedDate)),
            _sut.GetColumn<FirstLevelEntity>(nameof(FirstLevelEntity.FirstIntNullable)),
            _sut.GetColumn<FirstLevelEntity>(nameof(FirstLevelEntity.UpdatedDate)),
            _sut.GetColumn<SecondLevelEntity>(nameof(SecondLevelEntity.UpdatedDate)),
            _sut.GetColumn<SecondLevelEntity>(nameof(SecondLevelEntity.UpdatedDate)),
            _sut.GetColumn<SecondLevelEntity>(nameof(SecondLevelEntity.AnotherSecondString)),
            _sut.GetColumn<SecondLevelEntity>(nameof(SecondLevelEntity.AnotherSecondString)),
            _sut.GetColumn<SecondLevelEntity>(nameof(SecondLevelEntity.SomeSecondDecimal))
        };

        // Assert
        results.Should()
            .ContainInOrder(
                "first_int_nullable",
                "updated_date",
                "first_int_nullable",
                "updated_date",
                "updated_date",
                "updated_date",
                "another_second_string",
                "another_second_string",
                "some_second_decimal");
    }

    [Fact]
    public void GivenGetTableName_ShouldReturnsProperSchemaEveryTime()
    {
        // Arrange / Act
        var results = new[]
        {
            _sut.GetSchema<FirstLevelEntity>(), _sut.GetSchema<FirstLevelEntity>(),
            _sut.GetSchema<SecondLevelEntity>(), _sut.GetSchema<FirstLevelEntity>(),
            _sut.GetSchema<AttributeEntityLog>(), _sut.GetSchema<SecondLevelEntity>(),
            _sut.GetSchema<ThirdLevelEntity>()
        };

        // Assert
        results.Should()
            .ContainInOrder(
                null,
                null,
                null,
                null,
                "log",
                null,
                null);
    }

    [Fact]
    public void GivenGetTableName_ShouldReturnsProperTableNameEveryTime()
    {
        // Arrange / Act
        var results = new[]
        {
            _sut.GetTableName<FirstLevelEntity>(), _sut.GetTableName<FirstLevelEntity>(),
            _sut.GetTableName<SecondLevelEntity>(), _sut.GetTableName<FirstLevelEntity>(),
            _sut.GetTableName<SecondLevelEntity>(), _sut.GetTableName<ThirdLevelEntity>()
        };

        // Assert
        results.Should()
            .ContainInOrder(
                "first_level_entity",
                "first_level_entity",
                "second_level_entity",
                "first_level_entity",
                "second_level_entity",
                "third_level_entity");
    }
}
