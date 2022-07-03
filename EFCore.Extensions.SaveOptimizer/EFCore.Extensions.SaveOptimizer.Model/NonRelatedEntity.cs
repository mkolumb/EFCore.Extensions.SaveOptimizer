using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Model;

public class NonRelatedEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid NonRelatedEntityId { get; set; }

    [Required]
    public string? SomeNonNullableStringProperty { get; set; }

    public string? SomeNullableStringProperty { get; set; }

    [Required]
    public int? SomeNonNullableIntProperty { get; set; }

    public int? SomeNullableIntProperty { get; set; }

    [Required]
    [Precision(12, 6)]
    public decimal? SomeNonNullableDecimalProperty { get; set; }

    [Precision(12, 6)]
    public decimal? SomeNullableDecimalProperty { get; set; }

    [Required]
    public DateTimeOffset? SomeNonNullableDateTimeProperty { get; set; }

    public DateTimeOffset? SomeNullableDateTimeProperty { get; set; }

    [Required]
    public bool? SomeNonNullableBooleanProperty { get; set; }

    [ConcurrencyCheck]
    public DateTimeOffset? ConcurrencyToken { get; set; }
}
