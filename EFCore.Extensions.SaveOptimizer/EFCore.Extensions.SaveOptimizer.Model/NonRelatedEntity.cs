using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public decimal? SomeNonNullableDecimalProperty { get; set; }

    public decimal? SomeNullableDecimalProperty { get; set; }

    [Required]
    public DateTime? SomeNonNullableDateTimeProperty { get; set; }

    public DateTime? SomeNullableDateTimeProperty { get; set; }

    [Required]
    public bool? SomeNonNullableBooleanProperty { get; set; }

    [ConcurrencyCheck]
    public DateTime? ConcurrencyToken { get; set; }
}
