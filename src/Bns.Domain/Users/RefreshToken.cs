using Bns.Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.Users;

public class RefreshToken :Entity<RefreshTokenId>
{
    public User User { get; set; } // Владелец токена
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public UserId UserId { get; set; } // Внешний ключ к пользователю
    [Column("token")]
    public string Token { get; set; }
    [Column("expires")]
    public DateTime Expires { get; set; }
    [Column("created")]
    public DateTime Created { get; set; }
    [Column("created_by_ip")]
    public string CreatedByIp { get; set; }
    [Column("revoked")]
    public DateTime? Revoked { get; set; }
    [Column("revoked_by_ip")]
    public string RevokedByIp { get; set; }
    [Column("replaced_by_token")]
    public string ReplacedByToken { get; set; }
    [Column("reason_revoked")]
    public string ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}
