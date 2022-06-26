using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public abstract class BaseQueryBuilder : IQueryBuilder
{
    private readonly IDictionary<string, object?> _bindings;
    private readonly StringBuilder _builder;
    private readonly CaseType _caseType;
    private readonly IReadOnlyDictionary<ClauseType, string> _clauses;
    private bool _whereAdded;

    protected BaseQueryBuilder(IReadOnlyDictionary<ClauseType, string> clauses, CaseType caseType = CaseType.Normal)
    {
        _clauses = clauses;
        _caseType = caseType;
        _builder = new StringBuilder();
        _bindings = new Dictionary<string, object?>();
    }

    public IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, object?>> data)
    {
        _builder.Append($"{_clauses[ClauseType.Insert]} {GetTableName(tableName)} (");

        var idx = 0;

        foreach (var key in data[0].Keys)
        {
            _builder.Append(idx > 0
                ? $", {_clauses[ClauseType.ValueEscapeLeft]}{key}{_clauses[ClauseType.ValueEscapeRight]}"
                : $"{_clauses[ClauseType.ValueEscapeLeft]}{key}{_clauses[ClauseType.ValueEscapeRight]}");

            idx++;
        }

        _builder.Append(data.Count == 1 ? $") {_clauses[ClauseType.ValuesOne]}" : $") {_clauses[ClauseType.Values]}");

        for (var i = 0; i < data.Count; i++)
        {
            var valueSetLeft =
                data.Count == 1 ? _clauses[ClauseType.ValueSetOneLeft] : _clauses[ClauseType.ValueSetLeft];

            var valueSetRight = _clauses[ClauseType.ValueSetRight];

            if (data.Count == 1)
            {
                valueSetRight = _clauses[ClauseType.ValueSetOneRight];
            }
            else if (data.Count == i + 1)
            {
                valueSetRight = _clauses[ClauseType.ValueSetRightLast];
            }

            _builder.Append(i > 0 ? $"{_clauses[ClauseType.ValueSetSeparator]}{valueSetLeft}" : valueSetLeft);

            idx = 0;

            foreach (var key in data[i].Keys)
            {
                AppendValueIn(data[i][key], idx > 0);

                idx++;
            }

            _builder.Append(valueSetRight);
        }

        return this;
    }

    public IQueryBuilder Update(string tableName, IDictionary<string, object?> data)
    {
        _builder.Append($"{_clauses[ClauseType.Update]} {GetTableName(tableName)} {_clauses[ClauseType.Set]} ");

        var idx = 0;

        foreach (var (key, value) in data)
        {
            AppendValue(key, value, idx > 0);

            idx++;
        }

        return this;
    }

    public IQueryBuilder Delete(string tableName)
    {
        _builder.Append($"{_clauses[ClauseType.Delete]} {GetTableName(tableName)}");

        return this;
    }

    public IQueryBuilder Where(IDictionary<string, object?>? filter)
    {
        if (filter == null)
        {
            return this;
        }

        foreach (var (key, value) in filter)
        {
            if (_whereAdded)
            {
                _builder.Append($" {_clauses[ClauseType.And]} ");
            }
            else
            {
                _builder.Append($" {_clauses[ClauseType.Where]} ");

                _whereAdded = true;
            }

            AppendValue(key, value, false);
        }

        return this;
    }

    public IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results)
    {
        HashSet<DataGroupModel> data = DataGroupModel.CreateDataGroup(results, keys);

        AppendFilter(data);

        return this;
    }

    public SqlCommandModel Build()
    {
        var sql = _builder.ToString();

        switch (_caseType)
        {
            case CaseType.Normal:
                break;
            case CaseType.Lowercase:
                sql = sql
                    .ToLower()
                    .Replace(_clauses[ClauseType.ParameterPrefix].ToLower(), _clauses[ClauseType.ParameterPrefix]);
                break;
            case CaseType.Uppercase:
                sql = sql
                    .ToUpper()
                    .Replace(_clauses[ClauseType.ParameterPrefix].ToUpper(), _clauses[ClauseType.ParameterPrefix]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new SqlCommandModel { NamedBindings = _bindings, Sql = sql };
    }

    private string GetTableName(string tableName) =>
        $"{_clauses[ClauseType.ValueEscapeLeft]}{tableName.Replace(".", _clauses[ClauseType.TableEscape])}{_clauses[ClauseType.ValueEscapeRight]}";

    private void AppendValue(string key, object? value, bool comma)
    {
        _builder.Append(comma
            ? $", {_clauses[ClauseType.ValueEscapeLeft]}{key}{_clauses[ClauseType.ValueEscapeRight]} = {_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}"
            : $"{_clauses[ClauseType.ValueEscapeLeft]}{key}{_clauses[ClauseType.ValueEscapeRight]} = {_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}");

        _bindings.Add($"{_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}", value);
    }

    private void AppendValueIn(object? value, bool comma)
    {
        _builder.Append(comma
            ? $", {_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}"
            : $"{_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}");

        _bindings.Add($"{_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}", value);
    }

    private void AppendFilter(HashSet<DataGroupModel> data)
    {
        if (_whereAdded)
        {
            _builder.Append($" {_clauses[ClauseType.And]} ");
        }
        else
        {
            _builder.Append($" {_clauses[ClauseType.Where]} ");

            _whereAdded = true;
        }

        var idx = 0;

        DataGroupModel firstItem = data.First();

        if (!firstItem.NestedItems.Any())
        {
            if (data.Count > 1)
            {
                _builder.Append(
                    $"{_clauses[ClauseType.ValueEscapeLeft]}{firstItem.Key}{_clauses[ClauseType.ValueEscapeRight]} {_clauses[ClauseType.In]} ");

                _builder.Append(_clauses[ClauseType.RangeLeft]);

                foreach (DataGroupModel item in data)
                {
                    AppendValueIn(item.Value, idx > 0);

                    idx++;
                }

                _builder.Append(_clauses[ClauseType.RangeRight]);
            }
            else
            {
                AppendValue(firstItem.Key, firstItem.Value, false);
            }

            return;
        }

        _builder.Append(_clauses[ClauseType.RangeLeft]);

        idx = 0;

        foreach (DataGroupModel item in data)
        {
            if (idx > 0)
            {
                _builder.Append($" {_clauses[ClauseType.Or]} ");
            }

            _builder.Append(_clauses[ClauseType.RangeLeft]);

            AppendValue(item.Key, item.Value, false);

            if (item.NestedItems.Any())
            {
                AppendFilter(item.NestedItems);
            }

            _builder.Append(_clauses[ClauseType.RangeRight]);

            idx++;
        }

        _builder.Append(_clauses[ClauseType.RangeRight]);
    }
}
