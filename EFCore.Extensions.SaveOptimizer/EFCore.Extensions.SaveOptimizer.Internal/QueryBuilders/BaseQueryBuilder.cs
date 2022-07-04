using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

public abstract class BaseQueryBuilder : IQueryBuilder
{
    protected readonly IDictionary<SqlValueModel, HashSet<SqlParamModel>> Bindings;
    protected readonly StringBuilder Builder;
    protected readonly IReadOnlyDictionary<ClauseType, string> ClausesConfiguration;
    protected readonly QueryBuilderConfiguration Configuration;
    protected int BindingsCount;
    protected bool WhereAdded;

    protected BaseQueryBuilder(IReadOnlyDictionary<ClauseType, string> clausesConfiguration,
        QueryBuilderConfiguration? configuration)
    {
        ClausesConfiguration = clausesConfiguration;
        Configuration = configuration ?? new QueryBuilderConfiguration();
        Builder = new StringBuilder();
        Bindings = new Dictionary<SqlValueModel, HashSet<SqlParamModel>>();
    }

    public virtual IQueryBuilder Insert(string tableName, IReadOnlyList<IDictionary<string, SqlValueModel?>> data)
    {
        Builder.Append($"{ClausesConfiguration[ClauseType.Insert]} {GetTableName(tableName)} (");

        var idx = 0;

        foreach (var key in data[0].Keys)
        {
            Builder.Append(idx > 0
                ? $", {ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]}"
                : $"{ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]}");

            idx++;
        }

        Builder.Append(data.Count == 1
            ? $") {ClausesConfiguration[ClauseType.ValuesOne]}"
            : $") {ClausesConfiguration[ClauseType.Values]}");

        for (var i = 0; i < data.Count; i++)
        {
            var valueSetLeft = ClausesConfiguration[ClauseType.ValueSetLeft];
            var valueSetRight = ClausesConfiguration[ClauseType.ValueSetRight];

            if (data.Count == 1)
            {
                valueSetLeft = ClausesConfiguration[ClauseType.ValueSetOneLeft];
                valueSetRight = ClausesConfiguration[ClauseType.ValueSetOneRight];
            }
            else if (data.Count == i + 1)
            {
                valueSetRight = ClausesConfiguration[ClauseType.ValueSetRightLast];
            }

            Builder.Append(i > 0
                ? $"{ClausesConfiguration[ClauseType.ValueSetSeparator]}{valueSetLeft}"
                : valueSetLeft);

            idx = 0;

            foreach (var key in data[i].Keys)
            {
                AppendValueIn(data[i][key], idx > 0);

                idx++;
            }

            Builder.Append(valueSetRight);
        }

        return this;
    }

    public virtual IQueryBuilder Update(string tableName, IDictionary<string, SqlValueModel?> data)
    {
        Builder.Append(
            $"{ClausesConfiguration[ClauseType.Update]} {GetTableName(tableName)} {ClausesConfiguration[ClauseType.Set]} ");

        var idx = 0;

        foreach ((var key, SqlValueModel? value) in data)
        {
            AppendValue(key, value, idx > 0);

            idx++;
        }

        return this;
    }

    public virtual IQueryBuilder Delete(string tableName)
    {
        Builder.Append($"{ClausesConfiguration[ClauseType.Delete]} {GetTableName(tableName)}");

        return this;
    }

    public virtual IQueryBuilder Where(IDictionary<string, SqlValueModel?>? filter)
    {
        if (filter == null)
        {
            return this;
        }

        foreach ((var key, SqlValueModel? value) in filter)
        {
            if (WhereAdded)
            {
                Builder.Append($" {ClausesConfiguration[ClauseType.And]} ");
            }
            else
            {
                Builder.Append($" {ClausesConfiguration[ClauseType.Where]} ");

                WhereAdded = true;
            }

            AppendValue(key, value, false);
        }

        return this;
    }

    public virtual IQueryBuilder Where(IReadOnlyList<string> keys, IReadOnlyList<QueryDataModel> results)
    {
        HashSet<DataGroupModel> data = DataGroupModel.CreateDataGroup(results, keys);

        AppendFilter(data);

        return this;
    }

    public virtual ISqlCommandModel Build()
    {
        var sql = Builder.ToString().Trim();

        var queryEnding = ClausesConfiguration[ClauseType.QueryEnding];

        if (!string.IsNullOrWhiteSpace(queryEnding))
        {
            if (!sql.EndsWith(queryEnding))
            {
                sql = $"{sql}{queryEnding}".Trim();
            }

            while (sql.Contains($"{queryEnding}{queryEnding}"))
            {
                sql = sql.Replace($"{queryEnding}{queryEnding}", queryEnding);
            }
        }

        IReadOnlyCollection<SqlParamModel> parameters = Bindings.SelectMany(x => x.Value).ToArray();

        return new SqlCommandModel { Parameters = parameters, Sql = sql };
    }

    protected string GetTableName(string tableName)
    {
        var value =
            $"{ClausesConfiguration[ClauseType.ValueEscapeLeft]}{tableName.Replace(".", ClausesConfiguration[ClauseType.TableEscape])}{ClausesConfiguration[ClauseType.ValueEscapeRight]}";

        return ConvertCase(value);
    }

    protected string ConvertCase(string value) =>
        Configuration.CaseType switch
        {
            CaseType.Lowercase => value.ToLower(),
            CaseType.Uppercase => value.ToUpper(),
            _ => value
        };

    protected void AppendValue(string key, SqlValueModel? value, bool comma)
    {
        var paramKey = AppendParameterBinding(value);

        Builder.Append(comma
            ? $", {ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]} = {paramKey}"
            : $"{ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]} = {paramKey}");
    }

    protected void AppendValueIn(SqlValueModel? value, bool comma)
    {
        var paramKey = AppendParameterBinding(value);

        Builder.Append(comma ? $", {paramKey}" : $"{paramKey}");
    }

    protected virtual string CastParameter(string paramKey, SqlValueModel model) => paramKey;

    protected string AppendParameterBinding(SqlValueModel? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value.Value == null && ClausesConfiguration.ContainsKey(ClauseType.Null))
        {
            return CastParameter(ClausesConfiguration[ClauseType.Null], value);
        }

        if (!Bindings.ContainsKey(value))
        {
            Bindings.Add(value, new HashSet<SqlParamModel>());
        }

        HashSet<SqlParamModel> set = Bindings[value];

        if (Configuration.OptimizeParameters && set.Any())
        {
            return set.First().Key;
        }

        var paramKey = $"{ClausesConfiguration[ClauseType.ParameterPrefix]}{BindingsCount}";

        var castParameter = CastParameter(paramKey, value);

        set.Add(new SqlParamModel(paramKey, castParameter, value));

        BindingsCount++;

        return castParameter;
    }

    protected void AppendFilter(HashSet<DataGroupModel> data)
    {
        if (WhereAdded)
        {
            Builder.Append($" {ClausesConfiguration[ClauseType.And]} ");
        }
        else
        {
            Builder.Append($" {ClausesConfiguration[ClauseType.Where]} ");

            WhereAdded = true;
        }

        var idx = 0;

        DataGroupModel firstItem = data.First();

        if (!firstItem.NestedItems.Any())
        {
            if (data.Count > 1)
            {
                Builder.Append(
                    $"{ClausesConfiguration[ClauseType.ValueEscapeLeft]}{ConvertCase(firstItem.Key)}{ClausesConfiguration[ClauseType.ValueEscapeRight]} {ClausesConfiguration[ClauseType.In]} ");

                Builder.Append(ClausesConfiguration[ClauseType.RangeLeft]);

                foreach (DataGroupModel item in data)
                {
                    AppendValueIn(item.Value, idx > 0);

                    idx++;
                }

                Builder.Append(ClausesConfiguration[ClauseType.RangeRight]);
            }
            else
            {
                AppendValue(firstItem.Key, firstItem.Value, false);
            }

            return;
        }

        Builder.Append(ClausesConfiguration[ClauseType.RangeLeft]);

        idx = 0;

        foreach (DataGroupModel item in data)
        {
            if (idx > 0)
            {
                Builder.Append($" {ClausesConfiguration[ClauseType.Or]} ");
            }

            Builder.Append(ClausesConfiguration[ClauseType.RangeLeft]);

            AppendValue(item.Key, item.Value, false);

            if (item.NestedItems.Any())
            {
                AppendFilter(item.NestedItems);
            }

            Builder.Append(ClausesConfiguration[ClauseType.RangeRight]);

            idx++;
        }

        Builder.Append(ClausesConfiguration[ClauseType.RangeRight]);
    }
}
