using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public interface IQueryBuilder
{
    IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, SqlValueModel?>> data);

    IQueryBuilder Update(string tableName, IDictionary<string, SqlValueModel?> data);

    IQueryBuilder Delete(string tableName);

    IQueryBuilder Where(IDictionary<string, SqlValueModel?>? filter);

    IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results);

    ISqlCommandModel Build();
}
