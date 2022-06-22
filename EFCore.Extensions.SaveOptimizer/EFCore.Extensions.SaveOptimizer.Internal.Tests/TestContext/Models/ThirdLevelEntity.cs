using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;

public class ThirdLevelEntity
{
    [Key]
    public string ThirdLevelEntityId { get; set; }

    public string SomeField { get; set; }

    [Required]
    [ForeignKey(nameof(SecondLevelEntity))]
    public string SecondLevelEntityId { get; set; }

    public virtual SecondLevelEntity SecondLevelEntity { get; set; }

    public string PrimaryKeyValue => ThirdLevelEntityId;

    [ConcurrencyCheck]
    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }
}
