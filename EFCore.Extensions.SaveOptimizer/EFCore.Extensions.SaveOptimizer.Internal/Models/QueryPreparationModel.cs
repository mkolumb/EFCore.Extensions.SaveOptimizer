using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class QueryPreparationModel
{
    public IReadOnlyCollection<ISqlCommandModel> Queries { get; }

    public IReadOnlyCollection<EntityEntry> Entries { get; }

    public int ExpectedRows { get; }

    public QueryPreparationModel(IReadOnlyCollection<ISqlCommandModel> queries, IReadOnlyCollection<EntityEntry> entries, int expectedRows)
    {
        Queries = queries;
        ExpectedRows = expectedRows;
        Entries = entries;
    }
}
