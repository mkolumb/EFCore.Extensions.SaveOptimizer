using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public abstract class BaseQueryBuilder : IQueryBuilder
{
    private readonly IDictionary<string, object?> _bindings;
    private readonly StringBuilder _builder;
    private readonly CaseType _caseType;
    private readonly Dictionary<ClauseType, string> _clauses;

    protected BaseQueryBuilder(Dictionary<ClauseType, string> clauses, CaseType caseType = CaseType.Normal)
    {
        _clauses = clauses;
        _caseType = caseType;
        _builder = new StringBuilder();
        _bindings = new Dictionary<string, object?>();
    }

    public IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, object?>> data)
    {
        _builder.Append(
            $"{_clauses[ClauseType.Insert]} {_clauses[ClauseType.ValueEscapeLeft]}{tableName.Replace(".", _clauses[ClauseType.TableEscape])}{_clauses[ClauseType.ValueEscapeRight]} (");

        var idx = 0;

        foreach (var key in data[0].Keys)
        {
            _builder.Append(idx > 0
                ? $", {_clauses[ClauseType.ValueEscapeLeft]}{key}{_clauses[ClauseType.ValueEscapeRight]}"
                : $"{_clauses[ClauseType.ValueEscapeLeft]}{key}{_clauses[ClauseType.ValueEscapeRight]}");

            idx++;
        }

        _builder.Append(data.Count == 1 ? $") {_clauses[ClauseType.ValuesOne]}" : $") {_clauses[ClauseType.Values]}");

        idx = 0;

        for (var i = 0; i < data.Count; i++)
        {
            var valueSetLeft = data.Count == 1 ? _clauses[ClauseType.ValueSetOneLeft] : _clauses[ClauseType.ValueSetLeft];

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

            var jdx = 0;

            foreach (var key in data[i].Keys)
            {
                _builder.Append(jdx > 0
                    ? $", {_clauses[ClauseType.ParameterPrefix]}{idx}"
                    : $"{_clauses[ClauseType.ParameterPrefix]}{idx}");

                _bindings.Add($"{_clauses[ClauseType.ParameterPrefix]}{idx}", data[i][key]);

                jdx++;

                idx++;
            }

            _builder.Append(valueSetRight);
        }

        return this;
    }

    public IQueryBuilder Update(string tableName, IDictionary<string, object?> data) =>
        throw new NotImplementedException();

    public IQueryBuilder Delete(string tableName) => throw new NotImplementedException();

    public IQueryBuilder Where(IDictionary<string, object?>? filter) => throw new NotImplementedException();

    public IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results) =>
        throw new NotImplementedException();

    public SqlCommandModel Build()
    {
        var sql = _builder.ToString();

        switch (_caseType)
        {
            case CaseType.Normal:
                break;
            case CaseType.Lowercase:
                sql = sql.ToLower();

                sql = sql.Replace(_clauses[ClauseType.ParameterPrefix].ToLower(), _clauses[ClauseType.ParameterPrefix]);
                break;
            case CaseType.Uppercase:
                sql = sql.ToUpper();

                sql = sql.Replace(_clauses[ClauseType.ParameterPrefix].ToUpper(), _clauses[ClauseType.ParameterPrefix]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new SqlCommandModel { NamedBindings = _bindings, Sql = sql };
    }
}
