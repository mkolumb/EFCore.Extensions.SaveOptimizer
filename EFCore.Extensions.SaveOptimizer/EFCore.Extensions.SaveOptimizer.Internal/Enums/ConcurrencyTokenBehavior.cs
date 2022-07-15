// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Enums;

public enum ConcurrencyTokenBehavior
{
    /// <summary>
    /// It will throws exception when expected rows != affected rows
    /// </summary>
    ThrowException,

    /// <summary>
    /// It will skip entities with concurrency token changed, however will not throws exception
    /// </summary>
    SaveWhatPossible
}
