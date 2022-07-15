using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable All

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;

public class InsertablePrimaryKeyEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid InsertablePrimaryKeyEntityId { get; set; }

    public string? SomeProperty { get; set; }
}
