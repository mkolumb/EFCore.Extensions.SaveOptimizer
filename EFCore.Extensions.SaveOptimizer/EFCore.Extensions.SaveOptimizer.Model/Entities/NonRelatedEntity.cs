using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Model.Entities;

public class NonRelatedEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid NonRelatedEntityId { get; set; }

    [Required]
    public int Indexer { get; set; }

    [Required]
    public string? NonNullableString { get; set; }

    public string? NullableString { get; set; }

    [Required]
    public int? NonNullableInt { get; set; }

    public int? NullableInt { get; set; }

    [Required]
    [Precision(12, 6)]
    public decimal? NonNullableDecimal { get; set; }

    [Precision(12, 6)]
    public decimal? NullableDecimal { get; set; }

    [Required]
    public DateTimeOffset? NonNullableDateTime { get; set; }

    public DateTimeOffset? NullableDateTime { get; set; }

    [Required]
    public bool? NonNullableBoolean { get; set; }

    [ConcurrencyCheck]
    public DateTimeOffset? ConcurrencyToken { get; set; }
}
