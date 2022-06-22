using System.ComponentModel.DataAnnotations;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.LogEntities;

public class AttributeEntityLog
{
    [Key]
    public string LogId { get; set; }

    [Required]
    public string PrimaryKey { get; set; }

    [Required]
    public DateTime ChangeDate { get; set; }

    [Required]
    public string EntitySnapshot { get; set; }

    public ICollection<AttributeEntityPropertyLog> PropertyLogs { get; set; }
}
