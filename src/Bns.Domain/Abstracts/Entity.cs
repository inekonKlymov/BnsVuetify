using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.Abstracts;

public abstract class Entity<TId> : Entity
    where TId : notnull
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; } = default!;
}

public abstract class Entity
{
    //public object Id { get; set; } = default!;
}