using System.Text.Json;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Models;

public class DataGroupModelTests
{
    private readonly List<QueryDataModel> _items;

    public DataGroupModelTests()
    {
        _items = new List<QueryDataModel>();

        for (var i = 0; i < 10; i++)
        {
            for (var j = i; j < 12; j++)
            {
                for (var k = j; k < 14; k++)
                {
                    TestEntityInsiderModel entity = new()
                    {
                        InsiderId = $"i_{i}",
                        InsiderName = $"j_{j}",
                        InsiderValue = $"k_{k}"
                    };

                    QueryDataModel model = new(entity.GetType(),
                        EntityState.Added,
                        null,
                        "some",
                        new Dictionary<string, SqlValueModel?>
                        {
                            { nameof(entity.InsiderId), GetValueModel(entity.InsiderId) },
                            { nameof(entity.InsiderName), GetValueModel(entity.InsiderName) },
                            { nameof(entity.InsiderValue), GetValueModel(entity.InsiderValue) }
                        },
                        new HashSet<string>
                        {
                            nameof(entity.InsiderId), nameof(entity.InsiderName), nameof(entity.InsiderValue)
                        },
                        new Dictionary<string, SqlValueModel?>(),
                        3);

                    _items.Add(model);
                }
            }
        }

        for (var i = 0; i < 10; i++)
        {
            for (var j = i; j < 12; j++)
            {
                for (var k = j; k < 14; k++)
                {
                    TestEntityInsiderModel entity = new()
                    {
                        InsiderId = $"i_{i}",
                        InsiderName = $"j_{j}",
                        InsiderValue = $"k_{k}"
                    };

                    QueryDataModel model = new(entity.GetType(),
                        EntityState.Added,
                        null,
                        "some",
                        new Dictionary<string, SqlValueModel?>
                        {
                            { nameof(entity.InsiderId), GetValueModel(entity.InsiderId) },
                            { nameof(entity.InsiderName), GetValueModel(entity.InsiderName) },
                            { nameof(entity.InsiderValue), GetValueModel(entity.InsiderValue) }
                        },
                        new HashSet<string>
                        {
                            nameof(entity.InsiderId), nameof(entity.InsiderName), nameof(entity.InsiderValue)
                        },
                        new Dictionary<string, SqlValueModel?>(),
                        3);

                    _items.Add(model);
                }
            }
        }
    }

    [Fact]
    public void GivenCreateDataGroup_WhenOneLevelOfData_ShouldCreateProperDataGroup()
    {
        // Arrange
        HashSet<DataGroupModel> expected = new();

        for (var i = 0; i < 10; i++)
        {
            expected.Add(new DataGroupModel("InsiderId", GetValueModel($"i_{i}")));
        }

        // Act
        HashSet<DataGroupModel> result = DataGroupModel.CreateDataGroup(_items, new[] { "InsiderId" });

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
            DataGroupModel item = new("InsiderId", GetValueModel($"i_{i}"));

            for (var j = i; j < 12; j++)
            {
                DataGroupModel nested = new("InsiderName", GetValueModel($"j_{j}"));

                for (var k = j; k < 14; k++)
                {
                    nested.NestedItems.Add(new DataGroupModel("InsiderValue", GetValueModel($"k_{k}")));
                }

                item.NestedItems.Add(nested);
            }

            expected.Add(item);
        }

        // Act
        HashSet<DataGroupModel> result =
            DataGroupModel.CreateDataGroup(_items, new[] { "InsiderId", "InsiderName", "InsiderValue" });

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
            DataGroupModel item = new("InsiderId", GetValueModel($"i_{i}"));

            for (var j = i; j < 12; j++)
            {
                item.NestedItems.Add(new DataGroupModel("InsiderName", GetValueModel($"j_{j}")));
            }

            expected.Add(item);
        }

        // Act
        HashSet<DataGroupModel> result = DataGroupModel.CreateDataGroup(_items, new[] { "InsiderId", "InsiderName" });

        // Assert
        JsonSerializer.Serialize(result).Should().BeEquivalentTo(JsonSerializer.Serialize(expected));
    }

    private static SqlValueModel GetValueModel(object? value) => new(value, new PropertyTypeModel(null!, null!, null!));
}
