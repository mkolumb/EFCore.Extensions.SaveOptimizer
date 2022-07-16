using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class ExecutionResultModel : IExecutionResultModel
{
    public int AffectedRows { get; }

    public IReadOnlyList<EntityEntry> Entries { get; }

    private DbContext Context { get; }

    public int ExpectedRows { get; }

    public ExecutionResultModel(int affectedRows, QueryPreparationModel? model, DbContext context)
    {
        AffectedRows = affectedRows;
        Entries = model?.Entries ?? context.ChangeTracker.Entries().ToArray();
        ExpectedRows = model?.ExpectedRows ?? Entries.Count;
        Context = context;
    }

    public void ProcessAfterSave(AfterSaveBehavior? afterSaveBehavior)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (afterSaveBehavior)
        {
            case AfterSaveBehavior.ClearChanges:
                ClearChanges(Context);
                return;
            case AfterSaveBehavior.DetachSaved:
                DetachSaved(Entries);
                return;
            case AfterSaveBehavior.AcceptChanges:
                MarkTemporaryAsPermanent(Entries);
                AcceptChanges(Context);
                return;
            case AfterSaveBehavior.MarkTemporaryAsPermanent:
                MarkTemporaryAsPermanent(Entries);
                return;
            default:
                return;
        }
    }

    private static void ClearChanges(DbContext context) => context.ChangeTracker.Clear();

    private static void AcceptChanges(DbContext context) => context.ChangeTracker.AcceptAllChanges();

    private static void DetachSaved(IEnumerable<EntityEntry> entries)
    {
        foreach (EntityEntry entry in entries)
        {
            entry.State = EntityState.Detached;
        }
    }

    private static void MarkTemporaryAsPermanent(IEnumerable<EntityEntry> entries)
    {
        foreach (EntityEntry entry in entries)
        {
            foreach (PropertyEntry property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    property.IsTemporary = false;
                }
            }
        }
    }
}
