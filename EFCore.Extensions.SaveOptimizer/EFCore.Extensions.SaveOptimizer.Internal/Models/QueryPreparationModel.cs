using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public record QueryPreparationModel(IReadOnlyList<ISqlCommandModel> Queries,
    IReadOnlyList<EntityEntry> Entries,
    int ExpectedRows);
