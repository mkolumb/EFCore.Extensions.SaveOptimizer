using EFCore.Extensions.SaveOptimizer.Internal.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.LogEntities;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Extensions;

public class EntityTypeExtensionTests
{
    private readonly TestDataContext _sut;

    public EntityTypeExtensionTests()
    {
        DbContextOptionsBuilder options =
            new DbContextOptionsBuilder<TestDataContext>().UseInMemoryDatabase("in_memory_db");
        _sut = new TestDataContext(options.Options);
    }

    [Fact]
    public void GivenResolveEntityHierarchy_WhenCollection_ShouldResolveProperHierarchy()
    {
        // Arrange
        Random randomize = new(20);

        // Act / Assert
        for (var i = 0; i < 50; i++)
        {
            IEntityType[] entities = _sut.Model.GetEntityTypes().OrderBy(_ => randomize.Next()).ToArray();

            IDictionary<Type, int> result = entities.ResolveEntityHierarchy();

            result[typeof(FirstLevelEntity)].Should().BeLessThan(result[typeof(AttributeEntity)]);
            result[typeof(FirstLevelEntity)].Should().BeLessThan(result[typeof(SecondLevelEntity)]);
            result[typeof(FirstLevelEntity)].Should().BeLessThan(result[typeof(ThirdLevelEntity)]);

            result[typeof(SecondLevelEntity)].Should().BeLessThan(result[typeof(ThirdLevelEntity)]);

            result[typeof(AttributeEntityLog)].Should().BeLessThan(result[typeof(AttributeEntityPropertyLog)]);
        }
    }

    [Fact]
    public void GivenResolveEntityHierarchy_WhenModel_ShouldResolveProperHierarchy()
    {
        // Arrange / Act / Assert
        HashSet<Type> usedTypes = new()
        {
            typeof(FirstLevelEntity),
            typeof(SecondLevelEntity),
            typeof(ThirdLevelEntity),
            typeof(AttributeEntity),
            typeof(AttributeEntityLog),
            typeof(AttributeEntityPropertyLog)
        };

        for (var i = 0; i < 50; i++)
        {
            IDictionary<Type, int> result = _sut.Model.ResolveEntityHierarchy(usedTypes);

            result[typeof(FirstLevelEntity)].Should().BeLessThan(result[typeof(AttributeEntity)]);
            result[typeof(FirstLevelEntity)].Should().BeLessThan(result[typeof(SecondLevelEntity)]);
            result[typeof(FirstLevelEntity)].Should().BeLessThan(result[typeof(ThirdLevelEntity)]);

            result[typeof(SecondLevelEntity)].Should().BeLessThan(result[typeof(ThirdLevelEntity)]);

            result[typeof(AttributeEntityLog)].Should().BeLessThan(result[typeof(AttributeEntityPropertyLog)]);
        }
    }
}
