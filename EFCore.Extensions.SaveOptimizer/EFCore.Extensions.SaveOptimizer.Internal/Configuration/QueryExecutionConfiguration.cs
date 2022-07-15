using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;

namespace EFCore.Extensions.SaveOptimizer.Internal.Configuration;

public class QueryExecutionConfiguration
{
    public int? BatchSize { get; set; }

    public int? InsertBatchSize { get; set; }

    public int? UpdateBatchSize { get; set; }

    public int? DeleteBatchSize { get; set; }

    public int? ParametersLimit { get; set; }

    public ConcurrencyTokenBehavior? ConcurrencyTokenBehavior { get; set; }

    public bool? AutoTransactionEnabled { get; set; }

    public bool? AcceptAllChangesOnSuccess { get; set; }

    public IsolationLevel? AutoTransactionIsolationLevel { get; set; }

    public QueryBuilderConfiguration? BuilderConfiguration { get; set; }
}
