using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.LogEntities;

public class AttributeEntityPropertyLog
{
    [Key]
    public string LogPropertyId { get; set; }

    [Required]
    [ForeignKey(nameof(Log))]
    public string LogId { get; set; }

    public virtual AttributeEntityLog Log { get; set; }

    [Required]
    public string MemberName { get; set; }

    [Required]
    public string PropertyType { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}
