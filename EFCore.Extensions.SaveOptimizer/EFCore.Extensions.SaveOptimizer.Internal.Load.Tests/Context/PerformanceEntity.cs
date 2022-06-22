using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Context;

public class PerformanceEntity
{
    [Key]
    public string PrimaryKeyValue { get; set; }

    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedDate { get; set; }
}
