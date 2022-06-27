using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public abstract class BaseQueryBuilder : IQueryBuilder
{
    private readonly IDictionary<SqlValueModel, SqlParamModel> _bindings;
    private readonly StringBuilder _builder;
    private readonly IReadOnlyDictionary<ClauseType, string> _clauses;
    private readonly QueryBuilderConfiguration _configuration;
    private bool _whereAdded;

    protected BaseQueryBuilder(IReadOnlyDictionary<ClauseType, string> clauses,
        QueryBuilderConfiguration? configuration = null)
    {
        _clauses = clauses;
        _configuration = configuration ?? new QueryBuilderConfiguration();
        _builder = new StringBuilder();
        _bindings = new Dictionary<SqlValueModel, SqlParamModel>();
    }

    public IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, SqlValueModel?>> data)
    {
        _builder.Append($"{_clauses[ClauseType.Insert]} {GetTableName(tableName)} (");

        var idx = 0;

        foreach (var key in data[0].Keys)
        {
            _builder.Append(idx > 0
                ? $", {_clauses[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{_clauses[ClauseType.ValueEscapeRight]}"
                : $"{_clauses[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{_clauses[ClauseType.ValueEscapeRight]}");

            idx++;
        }

        _builder.Append(data.Count == 1 ? $") {_clauses[ClauseType.ValuesOne]}" : $") {_clauses[ClauseType.Values]}");

        for (var i = 0; i < data.Count; i++)
        {
            var valueSetLeft = _clauses[ClauseType.ValueSetLeft];
            var valueSetRight = _clauses[ClauseType.ValueSetRight];

            if (data.Count == 1)
            {
                valueSetLeft = _clauses[ClauseType.ValueSetOneLeft];
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

    public IQueryBuilder Update(string tableName, IDictionary<string, SqlValueModel?> data)
    {
        _builder.Append($"{_clauses[ClauseType.Update]} {GetTableName(tableName)} {_clauses[ClauseType.Set]} ");

        var idx = 0;

        foreach ((var key, SqlValueModel? value) in data)
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

    public IQueryBuilder Where(IDictionary<string, SqlValueModel?>? filter)
    {
        if (filter == null)
        {
            return this;
        }

        foreach ((var key, SqlValueModel? value) in filter)
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

    public ISqlCommandModel Build()
    {
        var sql = _builder.ToString().Trim();

        if (!string.IsNullOrWhiteSpace(_clauses[ClauseType.QueryEnding]) &&
            !sql.EndsWith(_clauses[ClauseType.QueryEnding]))
        {
            sql = $"{sql}{_clauses[ClauseType.QueryEnding]}";
        }

        return new SqlCommandModel { Parameters = _bindings.Values, Sql = sql };
    }

    private string GetTableName(string tableName)
    {
        var value =
            $"{_clauses[ClauseType.ValueEscapeLeft]}{tableName.Replace(".", _clauses[ClauseType.TableEscape])}{_clauses[ClauseType.ValueEscapeRight]}";

        return ConvertCase(value);
    }

    private string ConvertCase(string value) =>
        _configuration.CaseType switch
        {
            CaseType.Lowercase => value.ToLower(),
            CaseType.Uppercase => value.ToUpper(),
            _ => value
        };

    private void AppendValue(string key, SqlValueModel? value, bool comma)
    {
        var paramKey = AppendParameterBinding(value);

        _builder.Append(comma
            ? $", {_clauses[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{_clauses[ClauseType.ValueEscapeRight]} = {paramKey}"
            : $"{_clauses[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{_clauses[ClauseType.ValueEscapeRight]} = {paramKey}");
    }

    private void AppendValueIn(SqlValueModel? value, bool comma)
    {
        var paramKey = AppendParameterBinding(value);

        _builder.Append(comma ? $", {paramKey}" : $"{paramKey}");
    }

    private string AppendParameterBinding(SqlValueModel? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (_bindings.ContainsKey(value))
        {
            return _bindings[value].Key;
        }

        var paramKey = $"{_clauses[ClauseType.ParameterPrefix]}{_bindings.Count}";

        _bindings.Add(value, new SqlParamModel(paramKey, value));

        return paramKey;
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
                    $"{_clauses[ClauseType.ValueEscapeLeft]}{ConvertCase(firstItem.Key)}{_clauses[ClauseType.ValueEscapeRight]} {_clauses[ClauseType.In]} ");

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
