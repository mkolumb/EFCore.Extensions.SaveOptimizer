using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable All

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;

public class NonInsertablePrimaryKeyEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int NonInsertablePrimaryKeyEntityId { get; set; }

    public string? SomeProperty { get; set; }
}
