using System.Data.Common;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Services;

public class QueryExecutorService : IQueryExecutorService
{
    public int Execute(DbContext context,
        QueryExecutionConfiguration configuration,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout)
    {
        IRelationalConnection connection = GetConnection(context);

        ILogger logger = GetLogger(context);

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
        IRelationalConnection connection = GetConnection(context);

        ILogger logger = GetLogger(context);

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

        return command;
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

    private static IRelationalDatabaseFacadeDependencies GetDependencies(DbContext context)
    {
        IDatabaseFacadeDependenciesAccessor accessor = GetDependenciesAccessor(context);

        IDatabaseFacadeDependencies dependencies = accessor.Dependencies;

        if (dependencies is IRelationalDatabaseFacadeDependencies relationalDependencies)
        {
            return relationalDependencies;
        }

        throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
    }

    private static IRelationalConnection GetConnection(DbContext context) =>
        GetDependencies(context).RelationalConnection;

    private static ILogger GetLogger(DbContext context) => GetDependencies(context).CommandLogger.Logger;

    private static IDatabaseFacadeDependenciesAccessor GetDependenciesAccessor(DbContext context) => context.Database;
}
