using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestData;
using SqlKata.Compilers;

#pragma warning disable xUnit1026

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public abstract class BaseQueryBuilderTests
{
    private readonly Type _builderType;
    private readonly Compiler _compiler;
    private readonly Func<IQueryBuilder> _factory;

    public static IEnumerable<IEnumerable<object?>> InsertData => QueryBuilderTestData.InsertData;

    public static IEnumerable<IEnumerable<object?>> UpdateDeleteData => QueryBuilderTestData.UpdateDeleteData;

    protected BaseQueryBuilderTests(Compiler compiler, Func<IQueryBuilder> factory)
    {
        _compiler = compiler;
        _factory = factory;
        _builderType = _factory().GetType();
    }

    [Theory]
    [MemberData(nameof(InsertData))]
    public void GivenInsert_ShouldProduceSameResultsAsSqlKata(string tableName,
        List<IDictionary<string, SqlValueModel?>> data)
    {
        // Arrange
        var expected = new SqlKataBuilder(_compiler)
            .Insert(tableName, data)
            .Build()
            .CompileSql(_builderType);

        // Act
        var result = _factory()
            .Insert(tableName, data)
            .Build()
            .CompileSql(_builderType);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenUpdate_WhenFilterFirst_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, SqlValueModel?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, SqlValueModel?> data)
    {
        // Arrange
        var expected = new SqlKataBuilder(_compiler)
            .Update(tableName, data)
            .Where(filter)
            .Where(keys, queries)
            .Build()
            .CompileSql(_builderType);

        // Act
        var result = _factory()
            .Update(tableName, data)
            .Where(filter)
            .Where(keys, queries)
            .Build()
            .CompileSql(_builderType);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenDelete_WhenFilterFirst_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, SqlValueModel?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, SqlValueModel?> data)
    {
        // Arrange
        var expected = new SqlKataBuilder(_compiler)
            .Delete(tableName)
            .Where(filter)
            .Where(keys, queries)
            .Build()
            .CompileSql(_builderType);

        // Act
        var result = _factory()
            .Delete(tableName)
            .Where(filter)
            .Where(keys, queries)
            .Build()
            .CompileSql(_builderType);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenUpdate_WhenFilterLast_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, SqlValueModel?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, SqlValueModel?> data)
    {
        // Arrange
        var expected = new SqlKataBuilder(_compiler)
            .Update(tableName, data)
            .Where(keys, queries)
            .Where(filter)
            .Build()
            .CompileSql(_builderType);

        // Act
        var result = _factory()
            .Update(tableName, data)
            .Where(keys, queries)
            .Where(filter)
            .Build()
            .CompileSql(_builderType);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenDelete_WhenFilterLast_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, SqlValueModel?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, SqlValueModel?> data)
    {
        // Arrange
        var expected = new SqlKataBuilder(_compiler)
            .Delete(tableName)
            .Where(keys, queries)
            .Where(filter)
            .Build()
            .CompileSql(_builderType);

        // Act
        var result = _factory()
            .Delete(tableName)
            .Where(keys, queries)
            .Where(filter)
            .Build()
            .CompileSql(_builderType);

        // Assert
        result.Should().Be(expected);
    }
}
