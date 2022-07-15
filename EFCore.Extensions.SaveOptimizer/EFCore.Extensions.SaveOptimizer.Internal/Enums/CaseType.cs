namespace EFCore.Extensions.SaveOptimizer.Internal.Enums;

public enum CaseType
{
    /// <summary>
    /// It will not change column name case during execution 
    /// </summary>
    Normal,

    /// <summary>
    /// It will lowercase column name during execution
    /// </summary>
    Lowercase,

    /// <summary>
    /// It will uppercase column name during execution
    /// </summary>
    Uppercase
}
