namespace ims.Domain.Entities
{
    public class RefreshToken : BaseEntaity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public Guid UserId { get; set; }
    }
}
