using EFCore.Extensions.SaveOptimizer.Internal.Models;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Extensions;

public static class QueryExtensions
{
    public static Query WherePrimaryKeysIn(this Query query, string[] primaryKeyNames,
        IEnumerable<QueryDataModel> queryResults)
    {
        QueryDataModel[] results = queryResults.ToArray();

        Dictionary<string, Func<QueryDataModel, object?>> resolvers = new();

        foreach (var key in primaryKeyNames)
        {
            resolvers.Add(key, x => x.Data[key]);
        }

        HashSet<DataGroupModel> dataResult = DataGroupModel.CreateDataGroup(results, primaryKeyNames, resolvers);

        return WhereDataGroupItem(query, dataResult);
    }

    private static Query WhereDataGroupItem(Query query, HashSet<DataGroupModel> dataResult)
    {
        if (dataResult.Any(x => x.NestedItems.Any()))
        {
            query = query.Where(subQuery =>
            {
                foreach (DataGroupModel item in dataResult)
                {
                    subQuery = subQuery.OrWhere(p =>
                    {
                        p = p.Where(item.Key, item.Value);

                        p = WhereDataGroupItem(p, item.NestedItems);

                        return p;
                    });
                }

                return subQuery;
            });

            return query;
        }

        DataGroupModel firstItem = dataResult.First();

        var items = dataResult.Select(x => x.Value).ToArray();

        return items.Length > 1 ? query.WhereIn(firstItem.Key, items) : query.Where(firstItem.Key, items[0]);
    }
}
