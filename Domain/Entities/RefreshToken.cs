using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }

    public string TokenHash { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }

    public string CreatedByIp { get; set; } = string.Empty;
    public string? ReplacedByTokenHash { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired && !IsDeleted;
}
