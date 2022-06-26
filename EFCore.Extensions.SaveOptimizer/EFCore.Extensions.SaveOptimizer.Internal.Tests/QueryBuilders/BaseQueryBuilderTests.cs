using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestData;
using SqlKata.Net6.Compilers;

#pragma warning disable xUnit1026

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public abstract class BaseQueryBuilderTests
{
    private readonly Compiler _compiler;
    private readonly Func<IQueryBuilder> _factory;

    public static IEnumerable<IEnumerable<object?>> InsertData => QueryBuilderTestData.InsertData;

    public static IEnumerable<IEnumerable<object?>> UpdateDeleteData => QueryBuilderTestData.UpdateDeleteData;

    protected BaseQueryBuilderTests(Compiler compiler, Func<IQueryBuilder> factory)
    {
        _compiler = compiler;
        _factory = factory;
    }

    [Theory]
    [MemberData(nameof(InsertData))]
    public void GivenInsert_ShouldProduceSameResultsAsSqlKata(string tableName, List<IDictionary<string, object?>> data)
    {
        // Arrange
        SqlCommandModel expected = new SqlKataBuilder(_compiler)
            .Insert(tableName, data)
            .Build();

        // Act
        SqlCommandModel result = _factory()
            .Insert(tableName, data)
            .Build();

        // Assert
        result.Sql.Should().Be(expected.Sql);
        result.NamedBindings.Should().BeEquivalentTo(expected.NamedBindings);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenUpdate_WhenFilterFirst_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, object?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, object?> data)
    {
        // Arrange
        SqlCommandModel expected = new SqlKataBuilder(_compiler)
            .Update(tableName, data)
            .Where(filter)
            .Where(keys, queries)
            .Build();

        // Act
        SqlCommandModel result = _factory()
            .Update(tableName, data)
            .Where(filter)
            .Where(keys, queries)
            .Build();

        // Assert
        result.Sql.Should().Be(expected.Sql);
        result.NamedBindings.Should().BeEquivalentTo(expected.NamedBindings);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenDelete_WhenFilterFirst_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, object?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, object?> data)
    {
        // Arrange
        SqlCommandModel expected = new SqlKataBuilder(_compiler)
            .Delete(tableName)
            .Where(filter)
            .Where(keys, queries)
            .Build();

        // Act
        SqlCommandModel result = _factory()
            .Delete(tableName)
            .Where(filter)
            .Where(keys, queries)
            .Build();

        // Assert
        result.Sql.Should().Be(expected.Sql);
        result.NamedBindings.Should().BeEquivalentTo(expected.NamedBindings);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenUpdate_WhenFilterLast_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, object?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, object?> data)
    {
        // Arrange
        SqlCommandModel expected = new SqlKataBuilder(_compiler)
            .Update(tableName, data)
            .Where(keys, queries)
            .Where(filter)
            .Build();

        // Act
        SqlCommandModel result = _factory()
            .Update(tableName, data)
            .Where(keys, queries)
            .Where(filter)
            .Build();

        // Assert
        result.Sql.Should().Be(expected.Sql);
        result.NamedBindings.Should().BeEquivalentTo(expected.NamedBindings);
    }

    [Theory]
    [MemberData(nameof(UpdateDeleteData))]
    public void GivenDelete_WhenFilterLast_ShouldProduceSameResultsAsSqlKata(string tableName,
        Dictionary<string, object?> filter,
        IReadOnlyList<string> keys,
        IReadOnlyList<QueryDataModel> queries,
        Dictionary<string, object?> data)
    {
        // Arrange
        SqlCommandModel expected = new SqlKataBuilder(_compiler)
            .Delete(tableName)
            .Where(keys, queries)
            .Where(filter)
            .Build();

        // Act
        SqlCommandModel result = _factory()
            .Delete(tableName)
            .Where(keys, queries)
            .Where(filter)
            .Build();

        // Assert
        result.Sql.Should().Be(expected.Sql);
        result.NamedBindings.Should().BeEquivalentTo(expected.NamedBindings);
    }
}
