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

    public IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, SqlValueModel?>> data)
    {
        ICollection<string> columns = data[0].Keys;

        IEnumerable<ICollection<object?>> rows = data.Select(x => x.Values.Select(s => s?.Value).ToArray());

        _query = new Query(tableName).AsInsert(columns, rows);

        return this;
    }

    public IQueryBuilder Update(string tableName, IDictionary<string, SqlValueModel?> data)
    {
        Dictionary<string, object?> collection = data.ToDictionary(x => x.Key, x => x.Value?.Value);

        _query = new Query(tableName).AsUpdate(collection);

        return this;
    }

    public IQueryBuilder Delete(string tableName)
    {
        _query = new Query(tableName).AsDelete();

        return this;
    }

    public IQueryBuilder Where(IDictionary<string, SqlValueModel?>? filter)
    {
        Dictionary<string, object?>? collection = filter?.ToDictionary(x => x.Key, x => x.Value?.Value);

        if (collection != null && collection.Any())
        {
            _query = _query?.Where(collection);
        }

        return this;
    }

    public IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results)
    {
        _query = _query?.WherePrimaryKeysIn(keys, results);

        return this;
    }

    public ISqlCommandModel Build()
    {
        SqlResult? result = _compiler.Compile(_query);

        return new SqlKataCommandModel { Sql = result.Sql, NamedBindings = result.NamedBindings };
    }
}
