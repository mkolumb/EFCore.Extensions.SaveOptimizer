using System.ComponentModel.DataAnnotations;

// ReSharper disable All

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;

public class AttributeEntity
{
    [Key]
    public string AttributeEntityId { get; set; }

    public string Key { get; set; }
    public string Value { get; set; }

    [Required]
    public string FirstLevelEntityId { get; set; }

    public virtual FirstLevelEntity FirstLevelEntity { get; set; }

    public string PrimaryKeyValue => AttributeEntityId;

    [ConcurrencyCheck]
    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }
}
