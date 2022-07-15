namespace EFCore.Extensions.SaveOptimizer.Internal.Enums;

public enum AfterSaveBehavior
{
    /// <summary>
    /// It is default behavior, it will call ChangeTracker.Clear to prevent double save
    /// </summary>
    ClearChanges,

    /// <summary>
    /// It will call ChangeTracker.AcceptAllChanges, however it will not refresh temporary values - if you have auto increment key you should not use this
    /// </summary>
    AcceptChanges,

    /// <summary>
    /// It will mark all saved entities as detached
    /// </summary>
    DetachSaved,

    /// <summary>
    /// Do nothing
    /// </summary>
    DoNothing
}
