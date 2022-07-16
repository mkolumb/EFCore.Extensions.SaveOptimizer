using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

// ReSharper disable UnusedMemberInSuper.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public interface IExecutionResultModel
{
    int AffectedRows { get; }
    IReadOnlyList<EntityEntry> Entries { get; }
    int ExpectedRows { get; }
    void ProcessAfterSave(AfterSaveBehavior? afterSaveBehavior);
}
