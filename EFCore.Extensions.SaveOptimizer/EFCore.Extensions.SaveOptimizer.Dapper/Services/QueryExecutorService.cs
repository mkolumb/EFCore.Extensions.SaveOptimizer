using System.Data.Common;
using Dapper;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Dapper.Services;

public class QueryExecutorService : IQueryExecutorService
{
    public int Execute(DbContext context,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout)
    {
        IRelationalConnection connection = GetConnection(context);

        ILogger logger = GetLogger(context);

        CommandDefinition command = GetCommand(transaction, sql, timeout, logger);

        connection.Open();

        try
        {
            return connection.DbConnection.Execute(command);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when executing command: {Sql}", sql.Sql);

            throw;
        }
        finally
        {
            CleanupCommand(connection);
        }
    }

    public async Task<int> ExecuteAsync(DbContext context,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout,
        CancellationToken cancellationToken)
    {
        IRelationalConnection connection = GetConnection(context);

        ILogger logger = GetLogger(context);

        CommandDefinition command = GetCommand(transaction, sql, timeout, logger);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            return await connection.DbConnection.ExecuteAsync(command).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when executing command: {Sql}", sql.Sql);

            throw;
        }
        finally
        {
            await CleanupCommandAsync(connection).ConfigureAwait(false);
        }
    }

    private static CommandDefinition GetCommand(IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout,
        ILogger logger)
    {
        DbTransaction dbTransaction = transaction.GetDbTransaction();

        CommandDefinition command = new(
            sql.Sql,
            sql.NamedBindings,
            dbTransaction,
            timeout);

        logger.LogDebug("Executing command: {Sql}", sql.Sql);

        return command;
    }

    private static void CleanupCommand(IRelationalConnection connection) => connection.Close();

    private static async Task CleanupCommandAsync(IRelationalConnection connection) =>
        await connection.CloseAsync().ConfigureAwait(false);

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
