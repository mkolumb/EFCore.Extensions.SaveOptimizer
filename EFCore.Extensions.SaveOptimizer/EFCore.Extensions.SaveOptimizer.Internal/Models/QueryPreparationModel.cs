using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class QueryPreparationModel
{
    public IReadOnlyList<ISqlCommandModel> Queries { get; }

    public IReadOnlyList<EntityEntry> Entries { get; }

    public int ExpectedRows { get; }

    public QueryPreparationModel(IReadOnlyList<ISqlCommandModel> queries, IReadOnlyList<EntityEntry> entries, int expectedRows)
    {
        Queries = queries;
        ExpectedRows = expectedRows;
        Entries = entries;
    }
}
