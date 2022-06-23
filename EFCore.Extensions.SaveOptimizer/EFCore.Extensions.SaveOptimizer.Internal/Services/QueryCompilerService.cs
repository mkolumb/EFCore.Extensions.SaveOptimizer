using System.Text.Json;
using EFCore.Extensions.SaveOptimizer.Internal.Exceptions;
using EFCore.Extensions.SaveOptimizer.Internal.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Resolvers;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryCompilerService : IQueryCompilerService
{
    private readonly ICompilerWrapperResolver _compilerResolver;

    public QueryCompilerService(ICompilerWrapperResolver compilerResolver) => _compilerResolver = compilerResolver;

    public IEnumerable<SqlResult> Compile(IReadOnlyCollection<QueryDataModel> models, string providerName)
    {
        if (!models.Any())
        {
            return Array.Empty<SqlResult>();
        }

        HashSet<Type> entityTypes = new();
        HashSet<EntityState> queryTypes = new();
        HashSet<string?> schemas = new();
        HashSet<string> tables = new();
        HashSet<string> primaryKeyValidators = new();
        HashSet<string> primaryKeyNames = new();
        HashSet<string> batchKeys = new();
        Dictionary<string, List<Dictionary<string, object?>>> insertData = new();
        Dictionary<string, Dictionary<string, List<QueryDataModel>>> updateData = new();
        Dictionary<string, Dictionary<string, List<QueryDataModel>>> deleteData = new();

        foreach (QueryDataModel model in models)
        {
            entityTypes.Add(model.EntityType);
            queryTypes.Add(model.EntityState);
            schemas.Add(model.SchemaName);
            tables.Add(model.TableName);
            primaryKeyValidators.Add(model.PrimaryKeyNames.ToRepresentation(i => i));

            foreach (var primaryKeyName in model.PrimaryKeyNames)
            {
                primaryKeyNames.Add(primaryKeyName);
            }

            var batchKey = GetColumnsBatchKey(model);

            batchKeys.Add(batchKey);

            switch (model.EntityState)
            {
                case EntityState.Added:
                    {
                        if (!insertData.ContainsKey(batchKey))
                        {
                            insertData.Add(batchKey, new List<Dictionary<string, object?>>());
                        }

                        insertData[batchKey].Add(model.Data);
                        break;
                    }
                case EntityState.Modified:
                    {
                        var updateBatchKey = GetUpdateBatchKey(model);

                        if (!updateData.ContainsKey(batchKey))
                        {
                            updateData.Add(batchKey, new Dictionary<string, List<QueryDataModel>>());
                        }

                        if (!updateData[batchKey].ContainsKey(updateBatchKey))
                        {
                            updateData[batchKey].Add(updateBatchKey, new List<QueryDataModel>());
                        }

                        updateData[batchKey][updateBatchKey].Add(model);
                        break;
                    }
                case EntityState.Deleted:
                    {
                        var deleteBatchKey = GetDeleteBatchKey(model);

                        if (!deleteData.ContainsKey(batchKey))
                        {
                            deleteData.Add(batchKey, new Dictionary<string, List<QueryDataModel>>());
                        }

                        if (!deleteData[batchKey].ContainsKey(deleteBatchKey))
                        {
                            deleteData[batchKey].Add(deleteBatchKey, new List<QueryDataModel>());
                        }

                        deleteData[batchKey][deleteBatchKey].Add(model);
                        break;
                    }
            }
        }

        if (entityTypes.Count > 1)
        {
            throw new QueryCompileException("All entities should be the same type");
        }

        if (queryTypes.Count > 1)
        {
            throw new QueryCompileException("All queries should be the same type");
        }

        if (schemas.Count > 1)
        {
            throw new QueryCompileException("All schemas should be the same");
        }

        if (tables.Count > 1)
        {
            throw new QueryCompileException("All table should be the same");
        }

        if (primaryKeyValidators.Count > 1)
        {
            throw new QueryCompileException("All primary key should be the same");
        }

        var tableName = tables.First();

        var schema = schemas.First();

        EntityState queryType = queryTypes.First();

        if (!string.IsNullOrEmpty(schema))
        {
            tableName = $"{schema}.{tableName}";
        }

        List<Query> queries = new();

        var primaryKeys = primaryKeyNames.ToArray();

        foreach (var batchKey in batchKeys)
        {
            switch (queryType)
            {
                case EntityState.Added:
                    queries.Add(GetInsertQuery(insertData[batchKey], tableName));
                    break;
                case EntityState.Modified:
                    queries.AddRange(GetUpdateQueries(updateData[batchKey], tableName, primaryKeys));
                    break;
                case EntityState.Deleted:
                    queries.AddRange(GetDeleteQueries(deleteData[batchKey], tableName, primaryKeys));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(models), "Unrecognized query type");
            }
        }

        ICompilerWrapper compiler = _compilerResolver.Resolve(providerName);

        return queries.Select(compiler.Compile);
    }

    private static IEnumerable<Query> GetDeleteQueries(IDictionary<string, List<QueryDataModel>> queryResultGrouped,
        string tableName,
        IReadOnlyList<string> primaryKeyNames)
    {
        foreach ((_, List<QueryDataModel> queryResults) in queryResultGrouped)
        {
            QueryDataModel firstResult = queryResults[0];

            if (!primaryKeyNames.Any())
            {
                throw new QueryCompileException("Query needs to have primary keys");
            }

            Query? query = new Query(tableName)
                .AsDelete()
                .WherePrimaryKeysIn(primaryKeyNames, queryResults);

            IDictionary<string, object?> tokens = GetConcurrencyTokens(firstResult);

            if (tokens.Any())
            {
                query = query.Where(tokens);
            }

            yield return query;
        }
    }

    private static IEnumerable<Query> GetUpdateQueries(IDictionary<string, List<QueryDataModel>> queryResultGrouped,
        string tableName,
        IReadOnlyList<string> primaryKeyNames)
    {
        foreach ((_, List<QueryDataModel> queryResults) in queryResultGrouped)
        {
            QueryDataModel firstResult = queryResults[0];

            IDictionary<string, object?> data = GetUpdateParams(firstResult);

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

            IDictionary<string, object?> tokens = GetConcurrencyTokens(firstResult);

            if (tokens.Any())
            {
                query = query.Where(tokens);
            }

            yield return query;
        }
    }

    private static Query GetInsertQuery(IEnumerable<Dictionary<string, object?>> data, string tableName) =>
        new Query(tableName).AsInsert(data);

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

    private static IDictionary<string, object?> GetConcurrencyTokens(QueryDataModel queryResult) =>
        queryResult.ConcurrencyTokens ?? new Dictionary<string, object?>();

    private static IDictionary<string, object?> GetUpdateParams(QueryDataModel queryResult)
    {
        HashSet<string> primaryKeys = queryResult.PrimaryKeyNames;

        return queryResult.Data
            .Where(x => !primaryKeys.Contains(x.Key))
            .ToDictionary(x => x.Key, x => x.Value);
    }
}
