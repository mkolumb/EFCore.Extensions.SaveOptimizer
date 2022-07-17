using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFCore.Extensions.SaveOptimizer.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Model.Entities;

public class VariousTypeEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public int Id { get; set; }

    public string? SomeString { get; set; }

    public Guid? SomeGuid { get; set; }

    public bool? SomeBool { get; set; }

    public ExampleEnum? SomeEnum { get; set; }

    [Precision(5)]
    public DateTime? SomeDateTime { get; set; }

    [Precision(5)]
    public DateTimeOffset? SomeDateTimeOffset { get; set; }

    [Precision(5)]
    public TimeSpan? SomeTimeSpan { get; set; }

    public short? SomeShort { get; set; }
    public ushort? SomeUnsignedShort { get; set; }

    public int? SomeInt { get; set; }
    public uint? SomeUnsignedInt { get; set; }

    public long? SomeLong { get; set; }
    public ulong? SomeUnsignedLong { get; set; }

    public sbyte? SomeSignedByte { get; set; }
    public byte? SomeByte { get; set; }

    public float? SomeFloat { get; set; }

    public double? SomeDouble { get; set; }

    [Precision(12, 6)]
    public decimal? SomeDecimal { get; set; }
}
