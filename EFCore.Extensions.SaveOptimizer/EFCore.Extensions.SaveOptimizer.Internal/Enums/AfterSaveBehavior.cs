namespace EFCore.Extensions.SaveOptimizer.Internal.Enums;

public enum AfterSaveBehavior
{
    /// <summary>
    /// It will call ChangeTracker.Clear
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
    /// It will mark all temporary variables as permanent, also used when AcceptChanges behavior is set
    /// </summary>
    MarkTemporaryAsPermanent,

    /// <summary>
    /// It is default behavior, do nothing
    /// </summary>
    DoNothing
}
