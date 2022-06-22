using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Services;

public class QueryTranslatorServiceTests
{
    private readonly TestDataContext _context;
    private readonly DataContextModelWrapper _wrapper;

    private readonly QueryTranslatorService _target;

    public QueryTranslatorServiceTests()
    {
        DbContextOptionsBuilder<TestDataContext> options =
            new DbContextOptionsBuilder<TestDataContext>().UseInMemoryDatabase("in_memory_db")
                .UseSnakeCaseNamingConvention();
        _context = new TestDataContext(options.Options);

        _wrapper = new DataContextModelWrapper(() => _context);

        _target = new QueryTranslatorService();

        // TODO: tests for
        // - value generated on add
        // - auto increment primary key
        // - value converter
    }

    [Fact]
    public void GivenTranslate_WhenConcurrencyToken_ShouldPropertyTranslate()
    {
        // Arrange
        SecondLevelEntity entity = new()
        {
            SecondLevelEntityId = "aaa",
            SomeSecondDecimal = 15,
            CreatedDate = new DateTime(2020, 1, 1),
            UpdatedDate = new DateTime(2010, 01, 01)
        };

        EntityEntry<SecondLevelEntity> entry = _context.Entry(entity);
        entry.State = EntityState.Added;
        foreach (PropertyEntry property in entry.Properties)
        {
            property.IsModified = false;
        }

        entry.Property(x => x.SomeSecondDecimal).IsModified = true;
        entry.Property(x => x.CreatedDate).IsModified = true;

        // Act
        QueryDataModel? result = _target.Translate(_wrapper, entry);

        // Assert
        result.SchemaName.Should().BeNull();
        result.TableName.Should().Be("second_level_entity");
        result.EntityState.Should().Be(EntityState.Added);
        result.EntityType.Should().BeSameAs(typeof(SecondLevelEntity));
        result.PrimaryKeyNames[0].Should().Be("second_level_entity_id");
        result.ConcurrencyTokens.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "updated_date", new DateTime(2010, 01, 01) }
        });
        result.Data.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "some_second_decimal", 15 },
            { "created_date", new DateTime(2020, 1, 1) },
            { "second_level_entity_id", "aaa" },
            { "another_second_string", null },
            { "first_level_entity_id", null },
            { "some_second_long", 0L },
            { "some_second_string", null },
            { "updated_date", new DateTime(2010, 01, 01) }
        });
    }

    [Fact]
    public void GivenTranslate_WhenCreatedSecondLevelEntity_ShouldPropertyTranslate()
    {
        // Arrange
        SecondLevelEntity entity = new()
        {
            SecondLevelEntityId = "aaa",
            SomeSecondDecimal = 15,
            CreatedDate = new DateTime(2020, 1, 1)
        };

        EntityEntry<SecondLevelEntity> entry = _context.Entry(entity);
        entry.State = EntityState.Added;
        foreach (PropertyEntry property in entry.Properties)
        {
            property.IsModified = false;
        }

        entry.Property(x => x.SomeSecondDecimal).IsModified = true;
        entry.Property(x => x.CreatedDate).IsModified = true;

        // Act
        QueryDataModel? result = _target.Translate(_wrapper, entry);

        // Assert
        result.SchemaName.Should().BeNull();
        result.TableName.Should().Be("second_level_entity");
        result.EntityState.Should().Be(EntityState.Added);
        result.EntityType.Should().BeSameAs(typeof(SecondLevelEntity));
        result.PrimaryKeyNames[0].Should().Be("second_level_entity_id");
        result.ConcurrencyTokens.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "updated_date", null }
        });
        result.Data.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "some_second_decimal", 15 },
            { "created_date", new DateTime(2020, 1, 1) },
            { "second_level_entity_id", "aaa" },
            { "another_second_string", null },
            { "first_level_entity_id", null },
            { "some_second_long", 0L },
            { "some_second_string", null },
            { "updated_date", null }
        });
    }

    [Fact]
    public void GivenTranslate_WhenDeletedSecondLevelEntity_ShouldPropertyTranslate()
    {
        // Arrange
        SecondLevelEntity entity = new()
        {
            SecondLevelEntityId = "aaa",
            SomeSecondDecimal = 15,
            CreatedDate = new DateTime(2020, 1, 1)
        };

        EntityEntry<SecondLevelEntity> entry = _context.Entry(entity);
        entry.State = EntityState.Deleted;
        foreach (PropertyEntry property in entry.Properties)
        {
            property.IsModified = false;
        }

        entry.Property(x => x.SomeSecondDecimal).IsModified = true;
        entry.Property(x => x.CreatedDate).IsModified = true;

        // Act
        QueryDataModel? result = _target.Translate(_wrapper, entry);

        // Assert
        result.SchemaName.Should().BeNull();
        result.TableName.Should().Be("second_level_entity");
        result.EntityState.Should().Be(EntityState.Deleted);
        result.EntityType.Should().BeSameAs(typeof(SecondLevelEntity));
        result.PrimaryKeyNames[0].Should().Be("second_level_entity_id");
        result.ConcurrencyTokens.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "updated_date", null }
        });
        result.Data.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "some_second_decimal", 15 },
            { "created_date", new DateTime(2020, 1, 1) },
            { "second_level_entity_id", "aaa" },
            { "another_second_string", null },
            { "first_level_entity_id", null },
            { "some_second_long", 0L },
            { "some_second_string", null },
            { "updated_date", null }
        });
    }

    [Fact]
    public void GivenTranslate_WhenDetached_ShouldReturnsNull()
    {
        // Arrange
        SecondLevelEntity entity = new()
        {
            SecondLevelEntityId = "aaa",
            SomeSecondDecimal = 15,
            CreatedDate = new DateTime(2020, 1, 1)
        };

        EntityEntry<SecondLevelEntity> entry = _context.Entry(entity);
        foreach (PropertyEntry property in entry.Properties)
        {
            property.IsModified = false;
        }

        entry.Property(x => x.SomeSecondDecimal).IsModified = true;
        entry.Property(x => x.CreatedDate).IsModified = true;

        entry.State = EntityState.Detached;

        // Act
        QueryDataModel? result = _target.Translate(_wrapper, entry);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GivenTranslate_WhenUnchanged_ShouldReturnsNull()
    {
        // Arrange
        SecondLevelEntity entity = new()
        {
            SecondLevelEntityId = "aaa",
            SomeSecondDecimal = 15,
            CreatedDate = new DateTime(2020, 1, 1)
        };

        EntityEntry<SecondLevelEntity> entry = _context.Entry(entity);
        foreach (PropertyEntry property in entry.Properties)
        {
            property.IsModified = false;
        }

        entry.Property(x => x.SomeSecondDecimal).IsModified = true;
        entry.Property(x => x.CreatedDate).IsModified = true;

        entry.State = EntityState.Unchanged;

        // Act
        QueryDataModel? result = _target.Translate(_wrapper, entry);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GivenTranslate_WhenUpdatedSecondLevelEntity_ShouldPropertyTranslate()
    {
        // Arrange
        SecondLevelEntity entity = new()
        {
            SecondLevelEntityId = "aaa",
            SomeSecondDecimal = 15,
            CreatedDate = new DateTime(2020, 1, 1)
        };

        EntityEntry<SecondLevelEntity> entry = _context.Entry(entity);
        entry.State = EntityState.Modified;
        foreach (PropertyEntry property in entry.Properties)
        {
            property.IsModified = false;
        }

        entry.Property(x => x.SomeSecondDecimal).IsModified = true;
        entry.Property(x => x.CreatedDate).IsModified = true;

        // Act
        QueryDataModel? result = _target.Translate(_wrapper, entry);

        // Assert
        result.SchemaName.Should().BeNull();
        result.TableName.Should().Be("second_level_entity");
        result.EntityState.Should().Be(EntityState.Modified);
        result.EntityType.Should().BeSameAs(typeof(SecondLevelEntity));
        result.PrimaryKeyNames[0].Should().Be("second_level_entity_id");
        result.ConcurrencyTokens.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "updated_date", null }
        });
        result.Data.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "some_second_decimal", 15 },
            { "created_date", new DateTime(2020, 1, 1) },
            { "second_level_entity_id", "aaa" }
        });
    }
}
