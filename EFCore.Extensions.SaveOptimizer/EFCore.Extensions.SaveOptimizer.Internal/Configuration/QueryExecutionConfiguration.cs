namespace EFCore.Extensions.SaveOptimizer.Internal.Configuration;

public class QueryExecutionConfiguration
{
    public int? BatchSize { get; set; }

    public int? InsertBatchSize { get; set; }

    public int? UpdateBatchSize { get; set; }

    public int? DeleteBatchSize { get; set; }

    public int? ParametersLimit { get; set; }
}
