using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8625

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestData;

public static class QueryBuilderTestData
{
    public static IEnumerable<IEnumerable<object?>> UpdateDeleteData
    {
        get
        {
            Dictionary<string, object?>? filter = new() { { "id_7", 343 }, { "id_83", "any" } };

            Dictionary<string, object?> data = new() { { "id_1", 43 }, { "id_3", "small" } };

            foreach (IEnumerable<object?> p in GetValues(filter, data))
            {
                yield return p;
            }

            filter = null;

            foreach (IEnumerable<object?> p in GetValues(filter, data))
            {
                yield return p;
            }
        }
    }

    public static IEnumerable<IEnumerable<object?>> InsertData
    {
        get
        {
            List<IDictionary<string, object?>> data = new()
            {
                new Dictionary<string, object?> { { "id_1", "some_val" }, { "id_2", "some_val2" }, { "id_3", 45 } },
                new Dictionary<string, object?> { { "id_1", "some_val" }, { "id_2", "some_val2" }, { "id_3", 65 } },
                new Dictionary<string, object?> { { "id_1", "some_val" }, { "id_2", "some_val4" }, { "id_3", 75 } }
            };

            yield return new object?[] { "table_name", data };
            yield return new object?[] { "dbo.table_name", data };

            data = new List<IDictionary<string, object?>>
            {
                new Dictionary<string, object?> { { "id_1", "some_val" }, { "id_2", "some_val2" }, { "id_3", 45 } }
            };

            yield return new object?[] { "table_name", data };
            yield return new object?[] { "dbo.table_name", data };

            data = new List<IDictionary<string, object?>>
            {
                new Dictionary<string, object?> { { "id some", "some_val" }, { "id_2", "some_val2" }, { "id_3", 45 } }
            };

            yield return new object?[] { "table_name", data };
            yield return new object?[] { "dbo.table_name", data };
        }
    }

    private static IEnumerable<IEnumerable<object?>> GetValues(Dictionary<string, object?>? filter,
        Dictionary<string, object?> data)
    {
        var keys = new[] { "idx_1", "idx_3" };

        Dictionary<string, object?>[] values =
        {
            new() { { "idx_1", "some" }, { "idx_3", "some_3" } },
            new() { { "idx_1", "some2" }, { "idx_3", "some_5" } }
        };

        QueryDataModel[] queries = values
            .Select(v => new QueryDataModel(typeof(object), EntityState.Modified, null, null, v, null, null, 0))
            .ToArray();

        yield return new object?[] { "table_name", filter, keys, queries, data };
        yield return new object?[] { "dbo.table_name", filter, keys, queries, data };

        keys = new[] { "idx_1", "idx_3" };

        values = new Dictionary<string, object?>[]
        {
            new() { { "idx_1", "some2" }, { "idx_3", "some_3" } },
            new() { { "idx_1", "some2" }, { "idx_3", "some_5" } }
        };

        queries = values
            .Select(v => new QueryDataModel(typeof(object), EntityState.Modified, null, null, v, null, null, 0))
            .ToArray();

        yield return new object?[] { "table_name", filter, keys, queries, data };
        yield return new object?[] { "dbo.table_name", filter, keys, queries, data };

        keys = new[] { "idx_1" };

        values = new Dictionary<string, object?>[]
        {
            new() { { "idx_1", "some" }, { "idx_3", "some_3" } },
            new() { { "idx_1", "some2" }, { "idx_3", "some_5" } }
        };

        queries = values
            .Select(v => new QueryDataModel(typeof(object), EntityState.Modified, null, null, v, null, null, 0))
            .ToArray();

        yield return new object?[] { "table_name", filter, keys, queries, data };
        yield return new object?[] { "dbo.table_name", filter, keys, queries, data };
    }
}
