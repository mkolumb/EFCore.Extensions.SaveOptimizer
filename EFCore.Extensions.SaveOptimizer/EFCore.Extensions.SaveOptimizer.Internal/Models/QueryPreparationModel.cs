namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class QueryPreparationModel
{
    public IEnumerable<ISqlCommandModel> Queries { get; }

    public int ExpectedRows { get; }

    public QueryPreparationModel(IEnumerable<ISqlCommandModel> queries, int expectedRows)
    {
        Queries = queries;
        ExpectedRows = expectedRows;
    }
}
