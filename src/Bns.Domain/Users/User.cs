using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.Users;

//[Table("users")]
public class User : IdentityUser
{

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true; // Статус активный

    [Required, MaxLength(100)]
    [Column("full_name")]
    public string FullName { get; set; } = ""; // Полное имя

    [Column("delete_time")]
    public DateTime? DeleteTime { get; set; } // Soft delete: если не null — пользователь удалён
    
    [Column("create_time")]
    public DateTime CreateTime { get; set; }  // Дата создания пользователя

    [Column("modify_time")]
    public DateTime? ModifyTime { get; set; } // Дата последнего обновления пользователя

    
    [ForeignKey(nameof(Settings))]
    [Column("settings_id")]
    public UserSettingsId SettingsId { get; set; } // Внешний ключ к настройкам пользователя
    public UserSettings Settings { get; set; } = new(); // Настройки пользователя
    
}
