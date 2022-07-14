using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Extensions.SaveOptimizer.Model;

public class AutoIncrementPrimaryKeyEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public string? Some { get; set; }
}
