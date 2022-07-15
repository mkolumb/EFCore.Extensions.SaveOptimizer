using System.Data.Common;
using System.Reflection;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Services;

public class QueryExecutorService : IQueryExecutorService
{
    private readonly IDbContextDependencyResolverService _dbContextDependencyResolverService;

    public QueryExecutorService(IDbContextDependencyResolverService dbContextDependencyResolverService) =>
        _dbContextDependencyResolverService = dbContextDependencyResolverService;

    public int Execute(DbContext context,
        QueryExecutionConfiguration configuration,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout)
    {
        IRelationalConnection connection = _dbContextDependencyResolverService.GetConnection(context);

        ILogger logger = _dbContextDependencyResolverService.GetLogger(context);

        DbCommand command = GetCommand(transaction, sql, timeout, connection, logger);

        connection.Open();

        try
        {
            return command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when executing command: {Sql}", sql.Sql);

            throw;
        }
        finally
        {
            CleanupCommand(command, connection);
        }
    }

    public async Task<int> ExecuteAsync(DbContext context,
        QueryExecutionConfiguration configuration,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout,
        CancellationToken cancellationToken)
    {
        IRelationalConnection connection = _dbContextDependencyResolverService.GetConnection(context);

        ILogger logger = _dbContextDependencyResolverService.GetLogger(context);

        DbCommand command = GetCommand(transaction, sql, timeout, connection, logger);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when executing command: {Sql}", sql.Sql);

            throw;
        }
        finally
        {
            await CleanupCommandAsync(command, connection).ConfigureAwait(false);
        }
    }

    private static DbCommand GetCommand(IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout,
        IRelationalConnection connection,
        ILogger logger)
    {
        DbCommand command = connection.DbConnection.CreateCommand();

        command.CommandText = sql.Sql;

        if (sql.Parameters != null)
        {
            foreach (SqlParamModel param in sql.Parameters)
            {
                AddParameter(command, param);
            }
        }

        if (timeout.HasValue)
        {
            command.CommandTimeout = timeout.Value;
        }

        command.Transaction = transaction.GetDbTransaction();

        logger.LogDebug("Executing command: {Sql}", sql.Sql);

        return SetDbSpecificProperties(command);
    }

    private static DbCommand SetDbSpecificProperties(DbCommand command)
    {
        Type type = command.GetType();

        switch (type.Name)
        {
            case "OracleCommand":
                PropertyInfo? property = type.GetProperty("BindByName");

                if (property == null)
                {
                    return command;
                }

                property.SetValue(command, true);

                return command;
            default: return command;
        }
    }

    private static void AddParameter(DbCommand command, SqlParamModel param)
    {
        DbParameter parameter =
            param.SqlValueModel.PropertyTypeModel.ParameterResolver(command, param.Key, param.SqlValueModel.Value);

        command.Parameters.Add(parameter);
    }

    private static void CleanupCommand(
        DbCommand command,
        IRelationalConnection connection)
    {
        command.Parameters.Clear();
        command.Dispose();
        connection.Close();
    }

    private static async Task CleanupCommandAsync(
        DbCommand command,
        IRelationalConnection connection)
    {
        command.Parameters.Clear();
        await command.DisposeAsync().ConfigureAwait(false);
        await connection.CloseAsync().ConfigureAwait(false);
    }
}
