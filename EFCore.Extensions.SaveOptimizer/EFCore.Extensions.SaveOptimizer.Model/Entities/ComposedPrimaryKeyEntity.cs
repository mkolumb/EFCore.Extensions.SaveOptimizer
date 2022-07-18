using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Extensions.SaveOptimizer.Model.Entities;

public class ComposedPrimaryKeyEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public string? PrimaryFirst { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public string? PrimarySecond { get; set; }

    public string? Some { get; set; }
}
