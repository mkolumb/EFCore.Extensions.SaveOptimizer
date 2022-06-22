using System.Text.Json;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using FluentAssertions;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Models;

public class DataGroupModelTests
{
    private readonly List<TestEntityInsiderModel> _items;
    private readonly Dictionary<string, Func<TestEntityInsiderModel, object>> _resolvers;

    public DataGroupModelTests()
    {
        _items = new List<TestEntityInsiderModel>();

        for (var i = 0; i < 10; i++)
        {
            for (var j = i; j < 12; j++)
            {
                for (var k = j; k < 14; k++)
                {
                    _items.Add(new TestEntityInsiderModel
                    {
                        InsiderId = $"i_{i}",
                        InsiderName = $"j_{j}",
                        InsiderValue = $"k_{k}"
                    });
                }
            }
        }

        for (var i = 0; i < 10; i++)
        {
            for (var j = i; j < 12; j++)
            {
                for (var k = j; k < 14; k++)
                {
                    _items.Add(new TestEntityInsiderModel
                    {
                        InsiderId = $"i_{i}",
                        InsiderName = $"j_{j}",
                        InsiderValue = $"k_{k}"
                    });
                }
            }
        }

        _resolvers = new Dictionary<string, Func<TestEntityInsiderModel, object>>
        {
            { nameof(TestEntityInsiderModel.InsiderId), x => x.InsiderId },
            { nameof(TestEntityInsiderModel.InsiderName), x => x.InsiderName },
            { nameof(TestEntityInsiderModel.InsiderValue), x => x.InsiderValue }
        };
    }

    [Fact]
    public void GivenCreateDataGroup_WhenOneLevelOfData_ShouldCreateProperDataGroup()
    {
        // Arrange
        HashSet<DataGroupModel> expected = new();

        for (var i = 0; i < 10; i++)
        {
            expected.Add(new DataGroupModel("InsiderId", $"i_{i}"));
        }

        // Act
        HashSet<DataGroupModel> result = DataGroupModel.CreateDataGroup(_items, new[] { "InsiderId" }, _resolvers);

        // Assert
        JsonSerializer.Serialize(result).Should().BeEquivalentTo(JsonSerializer.Serialize(expected));
    }

    [Fact]
    public void GivenCreateDataGroup_WhenThreeLevelsOfData_ShouldCreateProperDataGroup()
    {
        // Arrange
        HashSet<DataGroupModel> expected = new();

        for (var i = 0; i < 10; i++)
        {
            DataGroupModel item = new("InsiderId", $"i_{i}");

            for (var j = i; j < 12; j++)
            {
                DataGroupModel nested = new("InsiderName", $"j_{j}");

                for (var k = j; k < 14; k++)
                {
                    nested.NestedItems.Add(new DataGroupModel("InsiderValue", $"k_{k}"));
                }

                item.NestedItems.Add(nested);
            }

            expected.Add(item);
        }

        // Act
        HashSet<DataGroupModel> result =
            DataGroupModel.CreateDataGroup(_items, new[] { "InsiderId", "InsiderName", "InsiderValue" }, _resolvers);

        // Assert
        JsonSerializer.Serialize(result).Should().BeEquivalentTo(JsonSerializer.Serialize(expected));
    }

    [Fact]
    public void GivenCreateDataGroup_WhenTwoLevelsOfData_ShouldCreateProperDataGroup()
    {
        // Arrange
        HashSet<DataGroupModel> expected = new();

        for (var i = 0; i < 10; i++)
        {
            DataGroupModel item = new("InsiderId", $"i_{i}");

            for (var j = i; j < 12; j++)
            {
                item.NestedItems.Add(new DataGroupModel("InsiderName", $"j_{j}"));
            }

            expected.Add(item);
        }

        // Act
        HashSet<DataGroupModel> result =
            DataGroupModel.CreateDataGroup(_items, new[] { "InsiderId", "InsiderName" }, _resolvers);

        // Assert
        JsonSerializer.Serialize(result).Should().BeEquivalentTo(JsonSerializer.Serialize(expected));
    }
}
