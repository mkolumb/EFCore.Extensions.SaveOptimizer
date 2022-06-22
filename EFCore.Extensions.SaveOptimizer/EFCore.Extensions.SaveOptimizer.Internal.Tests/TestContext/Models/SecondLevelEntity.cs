using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;

public class SecondLevelEntity
{
    [Key]
    public string SecondLevelEntityId { get; set; }

    [Required]
    [ForeignKey(nameof(FirstLevelEntity))]
    public string FirstLevelEntityId { get; set; }

    public virtual FirstLevelEntity FirstLevelEntity { get; set; }

    public string SomeSecondString { get; set; }
    public string AnotherSecondString { get; set; }
    public long SomeSecondLong { get; set; }
    public decimal SomeSecondDecimal { get; set; }

    [ConcurrencyCheck]
    public DateTime? UpdatedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ThirdLevelEntity> ThirdLevelEntities { get; set; } = new List<ThirdLevelEntity>();

    public string PrimaryKeyValue => SecondLevelEntityId;
}
