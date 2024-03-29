﻿using EFCore.Extensions.SaveOptimizer.Internal.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.LogEntities;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Wrapper;

public class DataContextModelWrapperTests
{
    private readonly DataContextModelWrapper _sut;

    public DataContextModelWrapperTests()
    {
        DbContextOptionsBuilder<TestDataContext> options =
            new DbContextOptionsBuilder<TestDataContext>().UseDynamicSqlLite();
        TestDataContext context = new(options.Options);
        _sut = new DataContextModelWrapper(() => context);
    }

    [Fact]
    public void GivenGetColumn_ShouldReturnsProperColumnEveryTime()
    {
        // Arrange / Act
        var results = new[]
        {
            _sut.GetColumn(typeof(FirstLevelEntity), nameof(FirstLevelEntity.FirstIntNullable)),
            _sut.GetColumn(typeof(FirstLevelEntity), nameof(FirstLevelEntity.UpdatedDate)),
            _sut.GetColumn(typeof(FirstLevelEntity), nameof(FirstLevelEntity.FirstIntNullable)),
            _sut.GetColumn(typeof(FirstLevelEntity), nameof(FirstLevelEntity.UpdatedDate)),
            _sut.GetColumn(typeof(SecondLevelEntity), nameof(SecondLevelEntity.UpdatedDate)),
            _sut.GetColumn(typeof(SecondLevelEntity), nameof(SecondLevelEntity.UpdatedDate)),
            _sut.GetColumn(typeof(SecondLevelEntity), nameof(SecondLevelEntity.AnotherSecondString)),
            _sut.GetColumn(typeof(SecondLevelEntity), nameof(SecondLevelEntity.AnotherSecondString)),
            _sut.GetColumn(typeof(SecondLevelEntity), nameof(SecondLevelEntity.SomeSecondDecimal))
        };

        // Assert
        results
            .Select(x => x.ColumnName)
            .Should()
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
            _sut.GetSchema(typeof(FirstLevelEntity)), _sut.GetSchema(typeof(FirstLevelEntity)),
            _sut.GetSchema(typeof(SecondLevelEntity)), _sut.GetSchema(typeof(FirstLevelEntity)),
            _sut.GetSchema(typeof(AttributeEntityLog)), _sut.GetSchema(typeof(SecondLevelEntity)),
            _sut.GetSchema(typeof(ThirdLevelEntity))
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
            _sut.GetTableName(typeof(FirstLevelEntity)), _sut.GetTableName(typeof(FirstLevelEntity)),
            _sut.GetTableName(typeof(SecondLevelEntity)), _sut.GetTableName(typeof(FirstLevelEntity)),
            _sut.GetTableName(typeof(SecondLevelEntity)), _sut.GetTableName(typeof(ThirdLevelEntity))
        };

        // Assert
        results.Should()
            .ContainInOrder(
                "first_level_entities",
                "first_level_entities",
                "second_level_entities",
                "first_level_entities",
                "second_level_entities",
                "third_level_entities");
    }
}
