using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata;
using SqlKata.Compilers;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Helpers;

public class SqlKataBuilder : IQueryBuilder
{
    private readonly Compiler _compiler;
    private Query? _query;

    public SqlKataBuilder(Compiler compiler) => _compiler = compiler;

    public IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, object?>> data)
    {
        ICollection<string> columns = data[0].Keys;

        IEnumerable<ICollection<object?>> rows = data.Select(x => x.Values);

        _query = new Query(tableName).AsInsert(columns, rows);

        return this;
    }

    public IQueryBuilder Update(string tableName, IDictionary<string, object?> data)
    {
        _query = new Query(tableName).AsUpdate(data);

        return this;
    }

    public IQueryBuilder Delete(string tableName)
    {
        _query = new Query(tableName).AsDelete();

        return this;
    }

    public IQueryBuilder Where(IDictionary<string, object?>? filter)
    {
        if (filter != null && filter.Any())
        {
            _query = _query?.Where(filter);
        }

        return this;
    }

    public IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results)
    {
        _query = _query?.WherePrimaryKeysIn(keys, results);

        return this;
    }

    public SqlCommandModel Build()
    {
        SqlResult? result = _compiler.Compile(_query);

        return new SqlCommandModel { Sql = result.Sql, NamedBindings = result.NamedBindings };
    }
}
