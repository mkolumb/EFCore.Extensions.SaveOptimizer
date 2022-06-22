using System.Text.Json;
using EFCore.Extensions.SaveOptimizer.Exceptions;
using EFCore.Extensions.SaveOptimizer.Extensions;
using EFCore.Extensions.SaveOptimizer.Models;
using EFCore.Extensions.SaveOptimizer.Wrappers;
using Microsoft.EntityFrameworkCore;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Services;

public class QueryCompilerService : IQueryCompilerService
{
    private readonly ICompilerWrapper _compiler;

    public QueryCompilerService(ICompilerWrapper compiler) => _compiler = compiler;

    public IEnumerable<SqlResult> Compile(IReadOnlyCollection<QueryDataModel> models)
    {
        if (!models.Any())
        {
            return Array.Empty<SqlResult>();
        }

        Type[] entityType = models.Select(x => x.EntityType).Distinct().ToArray();

        if (entityType.Length > 1)
        {
            throw new QueryCompileException("All entities should be the same type");
        }

        EntityState[] queryType = models.Select(x => x.EntityState).Distinct().ToArray();

        if (queryType.Length > 1)
        {
            throw new QueryCompileException("All queries should be the same type");
        }

        var schema = models.Select(x => x.SchemaName).Distinct().ToArray();

        if (schema.Length > 1)
        {
            throw new QueryCompileException("All schemas should be the same");
        }

        var table = models.Select(x => x.TableName).Distinct().ToArray();

        if (table.Length > 1)
        {
            throw new QueryCompileException("All table should be the same");
        }

        var primaryKeyValidator = models.Select(x => x.PrimaryKeyNames.ToRepresentation(x => x)).Distinct().ToArray();

        if (primaryKeyValidator.Length > 1)
        {
            throw new QueryCompileException("All primary key should be the same");
        }

        var tableName = table[0];

        if (!string.IsNullOrEmpty(schema[0]))
        {
            tableName = $"{schema[0]}.{tableName}";
        }

        List<Query> queries = new();

        IEnumerable<IGrouping<string, QueryDataModel>> columnsGrouped = models.GroupBy(GetColumnsBatchKey);

        var primaryKeys = models.SelectMany(x => x.PrimaryKeyNames).Distinct().ToArray();

        foreach (IGrouping<string, QueryDataModel> columnsGroup in columnsGrouped)
        {
            switch (queryType[0])
            {
                case EntityState.Added:
                    queries.Add(GetInsertQuery(columnsGroup, tableName));
                    break;
                case EntityState.Modified:
                    queries.AddRange(GetUpdateQueries(columnsGroup, tableName, primaryKeys));
                    break;
                case EntityState.Deleted:
                    queries.AddRange(GetDeleteQueries(columnsGroup, tableName, primaryKeys));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return queries.Select(_compiler.Compile);
    }

    private static IEnumerable<Query> GetDeleteQueries(IEnumerable<QueryDataModel> columnsGroup, string tableName,
        string[] primaryKeyNames)
    {
        IEnumerable<IGrouping<string, QueryDataModel>> queryResultGrouped = columnsGroup.GroupBy(GetDeleteBatchKey);

        foreach (IGrouping<string, QueryDataModel> queryResults in queryResultGrouped)
        {
            QueryDataModel firstResult = queryResults.First();

            if (!primaryKeyNames.Any())
            {
                throw new QueryCompileException("Query needs to have primary keys");
            }

            Query? query = new Query(tableName)
                .AsDelete()
                .WherePrimaryKeysIn(primaryKeyNames, queryResults);

            IDictionary<string, object> tokens = GetConcurrencyTokens(firstResult);

            if (tokens.Any())
            {
                query = query.Where(tokens);
            }

            yield return query;
        }
    }

    private static IEnumerable<Query> GetUpdateQueries(IEnumerable<QueryDataModel> columnsGroup, string tableName,
        string[] primaryKeyNames)
    {
        IEnumerable<IGrouping<string, QueryDataModel>> queryResultGrouped = columnsGroup.GroupBy(GetUpdateBatchKey);

        foreach (IGrouping<string, QueryDataModel> queryResults in queryResultGrouped)
        {
            QueryDataModel firstResult = queryResults.First();

            IDictionary<string, object> data = GetUpdateParams(firstResult);

            if (!data.Any())
            {
                throw new QueryCompileException("Data params could not be empty");
            }

            if (!primaryKeyNames.Any())
            {
                throw new QueryCompileException("Query needs to have primary keys");
            }

            Query? query = new Query(tableName)
                .AsUpdate(data)
                .WherePrimaryKeysIn(primaryKeyNames, queryResults);

            IDictionary<string, object> tokens = GetConcurrencyTokens(firstResult);

            if (tokens.Any())
            {
                query = query.Where(tokens);
            }

            yield return query;
        }
    }

    private static Query GetInsertQuery(IEnumerable<QueryDataModel> columnsGroup, string tableName)
    {
        Dictionary<string, object>[] data = columnsGroup.Select(queryDataResult => queryDataResult.Data).ToArray();

        var columns = data[0].Keys;

        var rows = data.Select(x => x.Values).ToArray();

        return new Query(tableName).AsInsert(columns, rows);
    }

    private static string GetColumnsBatchKey(QueryDataModel queryResult)
    {
        var columns = JsonSerializer.Serialize(queryResult.Data.Keys);

        return $"columns_{columns}";
    }

    private static string GetUpdateBatchKey(QueryDataModel queryResult)
    {
        var updateParams = JsonSerializer.Serialize(GetUpdateParams(queryResult));

        var tokens = JsonSerializer.Serialize(GetConcurrencyTokens(queryResult));

        return $"params_{updateParams}_tokens_{tokens}";
    }

    private static string GetDeleteBatchKey(QueryDataModel queryResult)
    {
        var tokens = JsonSerializer.Serialize(GetConcurrencyTokens(queryResult));

        return $"tokens_{tokens}";
    }

    private static IDictionary<string, object> GetConcurrencyTokens(QueryDataModel queryResult) =>
        queryResult.ConcurrencyTokens ?? new Dictionary<string, object>();

    private static IDictionary<string, object> GetUpdateParams(QueryDataModel queryResult)
    {
        var primaryKeys = queryResult.PrimaryKeyNames;

        return queryResult.Data
            .Where(x => !primaryKeys.Contains(x.Key))
            .ToDictionary(x => x.Key, x => x.Value);
    }
}
