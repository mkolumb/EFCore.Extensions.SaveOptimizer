using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public interface IQueryBuilder
{
    IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, object?>> data);

    IQueryBuilder Update(string tableName, IDictionary<string, object?> data);

    IQueryBuilder Delete(string tableName);

    IQueryBuilder Where(IDictionary<string, object?>? filter);

    IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results);

    SqlCommandModel Build();
}
