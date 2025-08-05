using Bns.Domain.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.Users;
public class Language :Entity<LanguageId>
{
    [Required, MaxLength(50)]
    [Column("name")]
    public required string Name { get; set; }

    [Required,MaxLength(2)]
    [Column("code")]
    public required string Code { get; set; }
}