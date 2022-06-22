using System.ComponentModel.DataAnnotations;

namespace EFCore.Extensions.SaveOptimizer.Tests.TestContext.Models;

public class FirstLevelEntity
{
    [Key]
    public string FirstLevelEntityId { get; set; }

    public int? FirstIntNullable { get; set; }
    public string FirstString { get; set; }
    public bool FirstBool { get; set; }

    public virtual ICollection<SecondLevelEntity> SecondLevelEntities { get; set; }

    public virtual ICollection<AttributeEntity> AttributeEntities { get; set; } = new List<AttributeEntity>();

    public string PrimaryKeyValue => FirstLevelEntityId;

    [ConcurrencyCheck]
    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }
}
