using EFCore.Extensions.SaveOptimizer.Internal.Exceptions;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Resolvers;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SqlKata;
using SqlKata.Compilers;
#pragma warning disable CS8625
#pragma warning disable CS8620

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Services;

public class QueryCompilerServiceTests
{
    private readonly QueryCompilerService _target;

    public QueryCompilerServiceTests()
    {
        Mock<ICompilerWrapperResolver> resolver = new();
        resolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(new CompilerWrapper(new PostgresCompiler()));

        _target = new QueryCompilerService(resolver.Object);
    }

    [Fact]
    public void GivenCompile_WhenDelete_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }, new[] { "order_id" }, null)
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result.ElementAt(0).Sql.Should().Be("DELETE FROM \"order\" WHERE \"order_id\" IN (@p0, @p1)");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new[] { 1, 2 }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenDeleteWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_1" } }),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 3 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } })
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should()
            .Be("DELETE FROM \"order\" WHERE \"order_id\" = @p0 AND \"order_concurrency_token\" = @p1");
        result.ElementAt(0).Bindings.Should()
            .BeEquivalentTo(new object[] { 1, "concurrency_token_1" }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should()
            .Be("DELETE FROM \"order\" WHERE \"order_id\" IN (@p0, @p1) AND \"order_concurrency_token\" = @p2");
        result.ElementAt(1).Bindings.Should()
            .BeEquivalentTo(new object[] { 2, 3, "concurrency_token_2" }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenDeleteWithMultiplePrimaryKeysWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "second_primary_key", 111 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_1" } }),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "second_primary_key", 112 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "second_primary_key", 333 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "second_primary_key", 444 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } })
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should()
            .Be(
                "DELETE FROM \"order\" WHERE ((\"order_id\" = @p0 AND \"second_primary_key\" = @p1)) AND \"order_concurrency_token\" = @p2");
        result.ElementAt(0).Bindings.Should()
            .BeEquivalentTo(new object[] { 1, 111, "concurrency_token_1" }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should()
            .Be(
                "DELETE FROM \"order\" WHERE ((\"order_id\" = @p0 AND \"second_primary_key\" = @p1) OR (\"order_id\" = @p2 AND \"second_primary_key\" IN (@p3, @p4))) AND \"order_concurrency_token\" = @p5");
        result.ElementAt(1).Bindings.Should()
            .BeEquivalentTo(new object[] { 2, 112, 3, 333, 444, "concurrency_token_2" }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenInsert_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }, new[] { "order_id" }, null)
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result.ElementAt(0).Sql.Should().Be("INSERT INTO \"order\" (\"order_id\") VALUES (@p0), (@p1)");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new[] { 1, 2 }, c => c.WithStrictOrdering());
    }


    [Fact]
    public void GivenCompile_WhenInsertWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_1" } }),
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } })
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result.ElementAt(0).Sql.Should().Be("INSERT INTO \"order\" (\"order_id\") VALUES (@p0), (@p1)");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new[] { 1, 2 }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentActions_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object>(), new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object>(), new[] { "order_id" }, null)
        };

        // Act
        Action result = () => _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().Throw<QueryCompileException>();
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentColumns_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object> { { "order_id2", 1 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object> { { "order_id3", 11 } }, new[] { "order_id" }, null)
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should().Be("INSERT INTO \"schema1\".\"order\" (\"order_id2\") VALUES (@p0)");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new[] { 1 }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should().Be("INSERT INTO \"schema1\".\"order\" (\"order_id3\") VALUES (@p0)");
        result.ElementAt(1).Bindings.Should().BeEquivalentTo(new[] { 11 }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentEntities_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object>(), new[] { "order_id" }, null),
            new(typeof(SecondLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object>(), new[] { "order_id" }, null)
        };

        // Act
        Action result = () => _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().Throw<QueryCompileException>();
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentPrimaryKey_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object>(), new[] { "order_id1" }, null),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object>(), new[] { "order_id2" }, null)
        };

        // Act
        Action result = () => _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().Throw<QueryCompileException>();
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentSchemas_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object>(), new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema2", "order",
                new Dictionary<string, object>(), new[] { "order_id" }, null)
        };

        // Act
        Action result = () => _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().Throw<QueryCompileException>();
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentTables_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order1",
                new Dictionary<string, object>(), new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order2",
                new Dictionary<string, object>(), new[] { "order_id" }, null)
        };

        // Act
        Action result = () => _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().Throw<QueryCompileException>();
    }

    [Fact]
    public void GivenCompile_WhenUpdate_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }, new[] { "order_id" }, null)
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new[] { 11, 1 }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1");
        result.ElementAt(1).Bindings.Should().BeEquivalentTo(new[] { 21, 2 }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenUpdateBatch_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "other", 21 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 4 }, { "other", 21 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 5 }, { "other", 11 } }, new[] { "order_id" }, null),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "other", 31 } }, new[] { "order_id" }, null)
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(3);
        result.ElementAt(0).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2)");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new[] { 11, 1, 5 }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2, @p3)");
        result.ElementAt(1).Bindings.Should().BeEquivalentTo(new[] { 21, 2, 3, 4 }, c => c.WithStrictOrdering());
        result.ElementAt(2).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1");
        result.ElementAt(2).Bindings.Should().BeEquivalentTo(new[] { 31, 6 }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenUpdateBatchWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "other", 21 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 4 }, { "other", 21 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 5 }, { "other", 11 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "other", 31 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } })
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(4);
        result.ElementAt(0).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2) AND \"order_concurrency_token\" = @p3");
        result.ElementAt(0).Bindings.Should().BeEquivalentTo(new object[] { 11, 1, 5, "concurrency_token_11" },
            c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2) AND \"order_concurrency_token\" = @p3");
        result.ElementAt(1).Bindings.Should().BeEquivalentTo(new object[] { 21, 2, 3, "concurrency_token_21" },
            c => c.WithStrictOrdering());
        result.ElementAt(2).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2");
        result.ElementAt(2).Bindings.Should()
            .BeEquivalentTo(new object[] { 21, 4, "concurrency_token_11" }, c => c.WithStrictOrdering());
        result.ElementAt(3).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2");
        result.ElementAt(3).Bindings.Should()
            .BeEquivalentTo(new object[] { 31, 6, "concurrency_token_31" }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenUpdateBatchWithMultiplePrimaryKeysConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "second_primary_key", 111 }, { "other", 11 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "second_primary_key", 112 }, { "other", 21 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "second_primary_key", 113 }, { "other", 21 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 4 }, { "second_primary_key", 114 }, { "other", 21 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 5 }, { "second_primary_key", 115 }, { "other", 11 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "second_primary_key", 116 }, { "other", 31 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "second_primary_key", 126 }, { "other", 31 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "second_primary_key", 146 }, { "other", 31 } },
                new[] { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } })
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(4);
        result.ElementAt(0).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" = @p2) OR (\"order_id\" = @p3 AND \"second_primary_key\" = @p4)) AND \"order_concurrency_token\" = @p5");
        result.ElementAt(0).Bindings.Should()
            .BeEquivalentTo(new object[] { 11, 1, 111, 5, 115, "concurrency_token_11" }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" = @p2) OR (\"order_id\" = @p3 AND \"second_primary_key\" = @p4)) AND \"order_concurrency_token\" = @p5");
        result.ElementAt(1).Bindings.Should()
            .BeEquivalentTo(new object[] { 21, 2, 112, 3, 113, "concurrency_token_21" }, c => c.WithStrictOrdering());
        result.ElementAt(2).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" = @p2)) AND \"order_concurrency_token\" = @p3");
        result.ElementAt(2).Bindings.Should().BeEquivalentTo(new object[] { 21, 4, 114, "concurrency_token_11" },
            c => c.WithStrictOrdering());
        result.ElementAt(3).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" IN (@p2, @p3, @p4))) AND \"order_concurrency_token\" = @p5");
        result.ElementAt(3).Bindings.Should()
            .BeEquivalentTo(new object[] { 31, 6, 116, 126, 146, "concurrency_token_31" }, c => c.WithStrictOrdering());
    }

    [Fact]
    public void GivenCompile_WhenUpdateWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }, new[] { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } })
        };

        // Act
        IEnumerable<SqlResult> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2");
        result.ElementAt(0).Bindings.Should()
            .BeEquivalentTo(new object[] { 11, 1, "concurrency_token_11" }, c => c.WithStrictOrdering());
        result.ElementAt(1).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2");
        result.ElementAt(1).Bindings.Should()
            .BeEquivalentTo(new object[] { 21, 2, "concurrency_token_21" }, c => c.WithStrictOrdering());
    }
}
