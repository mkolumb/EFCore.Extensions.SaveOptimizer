using EFCore.Extensions.SaveOptimizer.Internal.Exceptions;
using EFCore.Extensions.SaveOptimizer.Internal.Factories;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS8625
#pragma warning disable CS8620

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Services;

public class QueryCompilerServiceTests
{
    private readonly QueryCompilerService _target;

    public QueryCompilerServiceTests()
    {
        Mock<IQueryBuilderFactory> resolver = new();

        resolver.Setup(x => x.Query(It.IsAny<string>())).Returns(() => new PostgresQueryBuilder());

        _target = new QueryCompilerService(resolver.Object);
    }

    [Fact]
    public void GivenCompile_WhenDelete_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }.Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }.Map(), new HashSet<string> { "order_id" }, null, 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result.ElementAt(0).Sql.Should().Be("DELETE FROM \"order\" WHERE \"order_id\" IN (@p0, @p1);");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 1 }, { "@p1", 2 } });
    }

    [Fact]
    public void GivenCompile_WhenDeleteWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }.Map(), new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_1" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }.Map(), new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 3 } }.Map(), new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }.Map(), 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should()
            .Be("DELETE FROM \"order\" WHERE \"order_id\" = @p0 AND \"order_concurrency_token\" = @p1;");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 1 }, { "@p1", "concurrency_token_1" } });
        result.ElementAt(1).Sql.Should()
            .Be("DELETE FROM \"order\" WHERE \"order_id\" IN (@p0, @p1) AND \"order_concurrency_token\" = @p2;");
        result.ElementAt(1).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 2 }, { "@p1", 3 }, { "@p2", "concurrency_token_2" }
            });
    }

    [Fact]
    public void GivenCompile_WhenDeleteWithMultiplePrimaryKeysWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "second_primary_key", 111 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_1" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "second_primary_key", 112 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "second_primary_key", 333 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Deleted, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "second_primary_key", 444 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }.Map(), 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should()
            .Be(
                "DELETE FROM \"order\" WHERE ((\"order_id\" = @p0 AND \"second_primary_key\" = @p1)) AND \"order_concurrency_token\" = @p2;");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 1 }, { "@p1", 111 }, { "@p2", "concurrency_token_1" }
            });
        result.ElementAt(1).Sql.Should()
            .Be(
                "DELETE FROM \"order\" WHERE ((\"order_id\" = @p0 AND \"second_primary_key\" = @p1) OR (\"order_id\" = @p2 AND \"second_primary_key\" IN (@p3, @p4))) AND \"order_concurrency_token\" = @p5;");
        result.ElementAt(1).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 2 },
                { "@p1", 112 },
                { "@p2", 3 },
                { "@p3", 333 },
                { "@p4", 444 },
                { "@p5", "concurrency_token_2" }
            });
    }

    [Fact]
    public void GivenCompile_WhenInsert_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }.Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }.Map(), new HashSet<string> { "order_id" }, null, 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result.ElementAt(0).Sql.Should().Be("INSERT INTO \"order\" (\"order_id\") VALUES (@p0), (@p1);");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 1 }, { "@p1", 2 } });
    }


    [Fact]
    public void GivenCompile_WhenInsertWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 1 } }.Map(), new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_1" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object> { { "order_id", 2 } }.Map(), new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_2" } }.Map(), 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result.ElementAt(0).Sql.Should().Be("INSERT INTO \"order\" (\"order_id\") VALUES (@p0), (@p1);");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 1 }, { "@p1", 2 } });
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentActions_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1)
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
                new Dictionary<string, object> { { "order_id2", 1 } }.Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object> { { "order_id3", 11 } }.Map(), new HashSet<string> { "order_id" }, null, 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should().Be("INSERT INTO \"schema1\".\"order\" (\"order_id2\") VALUES (@p0);");
        result.ElementAt(0).NamedBindings.Should().BeEquivalentTo(new Dictionary<string, object> { { "@p0", 1 } });
        result.ElementAt(1).Sql.Should().Be("INSERT INTO \"schema1\".\"order\" (\"order_id3\") VALUES (@p0);");
        result.ElementAt(1).NamedBindings.Should().BeEquivalentTo(new Dictionary<string, object> { { "@p0", 11 } });
    }

    [Fact]
    public void GivenCompile_WhenResultsWithDifferentEntities_ShouldThrows()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(SecondLevelEntity), EntityState.Added, null, "order",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1)
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
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id1" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id2" }, null, 1)
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
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema2", "order",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1)
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
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Added, "schema1", "order2",
                new Dictionary<string, object>().Map(), new HashSet<string> { "order_id" }, null, 1)
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
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1;");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 11 }, { "@p1", 1 } });
        result.ElementAt(1).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1;");
        result.ElementAt(1).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 21 }, { "@p1", 2 } });
    }

    [Fact]
    public void GivenCompile_WhenUpdateBatch_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 4 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 5 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "other", 31 } }.Map(),
                new HashSet<string> { "order_id" }, null, 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(3);
        result.ElementAt(0).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2);");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 11 }, { "@p1", 1 }, { "@p2", 5 } });
        result.ElementAt(1).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2, @p3);");
        result.ElementAt(1).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 21 }, { "@p1", 2 }, { "@p2", 3 }, { "@p3", 4 } });
        result.ElementAt(2).Sql.Should().Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1;");
        result.ElementAt(2).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object> { { "@p0", 31 }, { "@p1", 6 } });
    }

    [Fact]
    public void GivenCompile_WhenUpdateBatchWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 4 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 5 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "other", 31 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } }.Map(), 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(4);
        result.ElementAt(0).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2) AND \"order_concurrency_token\" = @p3;");
        result.ElementAt(0).NamedBindings.Should().BeEquivalentTo(new Dictionary<string, object>
        {
            { "@p0", 11 }, { "@p1", 1 }, { "@p2", 5 }, { "@p3", "concurrency_token_11" }
        });
        result.ElementAt(1).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" IN (@p1, @p2) AND \"order_concurrency_token\" = @p3;");
        result.ElementAt(1).NamedBindings.Should().BeEquivalentTo(new Dictionary<string, object>
        {
            { "@p0", 21 }, { "@p1", 2 }, { "@p2", 3 }, { "@p3", "concurrency_token_21" }
        });
        result.ElementAt(2).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2;");
        result.ElementAt(2).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 21 }, { "@p1", 4 }, { "@p2", "concurrency_token_11" }
            });
        result.ElementAt(3).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2;");
        result.ElementAt(3).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 31 }, { "@p1", 6 }, { "@p2", "concurrency_token_31" }
            });
    }

    [Fact]
    public void GivenCompile_WhenUpdateBatchWithMultiplePrimaryKeysConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "second_primary_key", 111 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "second_primary_key", 112 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 3 }, { "second_primary_key", 113 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 4 }, { "second_primary_key", 114 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 5 }, { "second_primary_key", 115 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "second_primary_key", 116 }, { "other", 31 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "second_primary_key", 126 }, { "other", 31 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 6 }, { "second_primary_key", 146 }, { "other", 31 } }.Map(),
                new HashSet<string> { "order_id", "second_primary_key" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_31" } }.Map(), 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(4);
        result.ElementAt(0).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" = @p2) OR (\"order_id\" = @p3 AND \"second_primary_key\" = @p4)) AND \"order_concurrency_token\" = @p5;");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 11 },
                { "@p1", 1 },
                { "@p2", 111 },
                { "@p3", 5 },
                { "@p4", 115 },
                { "@p5", "concurrency_token_11" }
            });
        result.ElementAt(1).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" = @p2) OR (\"order_id\" = @p3 AND \"second_primary_key\" = @p4)) AND \"order_concurrency_token\" = @p5;");
        result.ElementAt(1).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 21 },
                { "@p1", 2 },
                { "@p2", 112 },
                { "@p3", 3 },
                { "@p4", 113 },
                { "@p5", "concurrency_token_21" }
            });
        result.ElementAt(2).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" = @p2)) AND \"order_concurrency_token\" = @p3;");
        result.ElementAt(2).NamedBindings.Should().BeEquivalentTo(new Dictionary<string, object>
        {
            { "@p0", 21 }, { "@p1", 4 }, { "@p2", 114 }, { "@p3", "concurrency_token_11" }
        });
        result.ElementAt(3).Sql.Should()
            .Be(
                "UPDATE \"order\" SET \"other\" = @p0 WHERE ((\"order_id\" = @p1 AND \"second_primary_key\" IN (@p2, @p3, @p4))) AND \"order_concurrency_token\" = @p5;");
        result.ElementAt(3).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 31 },
                { "@p1", 6 },
                { "@p2", 116 },
                { "@p3", 126 },
                { "@p4", 146 },
                { "@p5", "concurrency_token_31" }
            });
    }

    [Fact]
    public void GivenCompile_WhenUpdateWithConcurrencyToken_ShouldCompile()
    {
        // Arrange
        QueryDataModel[] queryResults =
        {
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 1 }, { "other", 11 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_11" } }.Map(), 1),
            new(typeof(FirstLevelEntity), EntityState.Modified, null, "order",
                new Dictionary<string, object> { { "order_id", 2 }, { "other", 21 } }.Map(),
                new HashSet<string> { "order_id" },
                new Dictionary<string, object> { { "order_concurrency_token", "concurrency_token_21" } }.Map(), 1)
        };

        // Act
        IEnumerable<ISqlCommandModel> result = _target.Compile(queryResults, string.Empty);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2;");
        result.ElementAt(0).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 11 }, { "@p1", 1 }, { "@p2", "concurrency_token_11" }
            });
        result.ElementAt(1).Sql.Should()
            .Be("UPDATE \"order\" SET \"other\" = @p0 WHERE \"order_id\" = @p1 AND \"order_concurrency_token\" = @p2;");
        result.ElementAt(1).NamedBindings.Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                { "@p0", 21 }, { "@p1", 2 }, { "@p2", "concurrency_token_21" }
            });
    }
}
