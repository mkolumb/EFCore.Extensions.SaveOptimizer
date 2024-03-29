﻿using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Exceptions;
using EFCore.Extensions.SaveOptimizer.Internal.Extensions;
using EFCore.Extensions.SaveOptimizer.Internal.Factories;
using EFCore.Extensions.SaveOptimizer.Internal.Helpers;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryCompilerService : IQueryCompilerService
{
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public QueryCompilerService(IQueryBuilderFactory queryBuilderFactory) => _queryBuilderFactory = queryBuilderFactory;

    public IEnumerable<ISqlCommandModel> Compile(IReadOnlyCollection<QueryDataModel> models, QueryBuilderConfiguration? configuration)
    {
        if (!models.Any())
        {
            return Array.Empty<ISqlCommandModel>();
        }

        HashSet<Type> entityTypes = new();
        HashSet<EntityState> queryTypes = new();
        HashSet<string?> schemas = new();
        HashSet<string> tables = new();
        HashSet<string> primaryKeyValidators = new();
        HashSet<string> primaryKeyNames = new();
        HashSet<string> batchKeys = new();
        Dictionary<string, List<Dictionary<string, SqlValueModel?>>> insertData = new();
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

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (model.EntityState)
            {
                case EntityState.Added:
                    {
                        if (!insertData.ContainsKey(batchKey))
                        {
                            insertData.Add(batchKey, new List<Dictionary<string, SqlValueModel?>>());
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

        List<ISqlCommandModel> queries = new();

        var primaryKeys = primaryKeyNames.ToArray();

        foreach (var batchKey in batchKeys)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (queryType)
            {
                case EntityState.Added:
                    queries.Add(GetInsertQuery(configuration, insertData[batchKey], tableName));
                    break;
                case EntityState.Modified:
                    queries.AddRange(GetUpdateQueries(configuration, updateData[batchKey], tableName, primaryKeys));
                    break;
                case EntityState.Deleted:
                    queries.AddRange(GetDeleteQueries(configuration, deleteData[batchKey], tableName, primaryKeys));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(models), "Unrecognized query type");
            }
        }

        return queries;
    }

    private IEnumerable<ISqlCommandModel> GetDeleteQueries(
        QueryBuilderConfiguration? configuration,
        IDictionary<string, List<QueryDataModel>> queryResultGrouped,
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

            IQueryBuilder builder = _queryBuilderFactory.Query(configuration)
                .Delete(tableName)
                .Where(primaryKeyNames, queryResults)
                .Where(firstResult.ConcurrencyTokens);

            yield return builder.Build();
        }
    }

    private IEnumerable<ISqlCommandModel> GetUpdateQueries(
        QueryBuilderConfiguration? configuration,
        IDictionary<string, List<QueryDataModel>> queryResultGrouped,
        string tableName,
        IReadOnlyList<string> primaryKeyNames)
    {
        foreach ((_, List<QueryDataModel> queryResults) in queryResultGrouped)
        {
            QueryDataModel firstResult = queryResults[0];

            IDictionary<string, SqlValueModel?> data = GetUpdateParams(firstResult);

            if (!data.Any())
            {
                throw new QueryCompileException("Data params could not be empty");
            }

            if (!primaryKeyNames.Any())
            {
                throw new QueryCompileException("Query needs to have primary keys");
            }

            IQueryBuilder builder = _queryBuilderFactory.Query(configuration)
                .Update(tableName, data)
                .Where(primaryKeyNames, queryResults)
                .Where(firstResult.ConcurrencyTokens);

            yield return builder.Build();
        }
    }

    private ISqlCommandModel GetInsertQuery(
        QueryBuilderConfiguration? configuration,
        IReadOnlyList<IDictionary<string, SqlValueModel?>> data,
        string tableName)
    {
        IQueryBuilder builder = _queryBuilderFactory.Query(configuration)
            .Insert(tableName, data);

        return builder.Build();
    }

    private static string GetColumnsBatchKey(QueryDataModel queryResult) => string.Join("_", queryResult.Data.Keys);

    private static string GetUpdateBatchKey(QueryDataModel queryResult)
    {
        StringBuilder builder = new();

        builder = AddUpdateParamsRepresentation(queryResult, builder);

        builder = AddTokensRepresentation(queryResult, builder);

        return builder.ToString();
    }

    private static string GetDeleteBatchKey(QueryDataModel queryResult)
    {
        StringBuilder builder = new();

        builder = AddTokensRepresentation(queryResult, builder);

        return builder.ToString();
    }

    private static StringBuilder AddUpdateParamsRepresentation(QueryDataModel queryResult, StringBuilder builder)
    {
        IDictionary<string, SqlValueModel?> data = GetUpdateParams(queryResult);

        foreach ((var key, SqlValueModel? value) in data)
        {
            builder.Append(SerializationHelper.Serialize(key, value?.Value));
        }

        return builder;
    }

    private static StringBuilder AddTokensRepresentation(QueryDataModel queryResult, StringBuilder builder)
    {
        if (queryResult.ConcurrencyTokens == null)
        {
            return builder;
        }

        foreach ((var key, SqlValueModel? value) in queryResult.ConcurrencyTokens)
        {
            builder.Append(SerializationHelper.Serialize(key, value?.Value));
        }

        return builder;
    }

    private static IDictionary<string, SqlValueModel?> GetUpdateParams(QueryDataModel queryResult)
    {
        Dictionary<string, SqlValueModel?> dict = new();

        foreach ((var key, SqlValueModel? value) in queryResult.Data)
        {
            if (queryResult.PrimaryKeyNames.Contains(key))
            {
                continue;
            }

            dict.Add(key, value);
        }

        return dict;
    }
}
