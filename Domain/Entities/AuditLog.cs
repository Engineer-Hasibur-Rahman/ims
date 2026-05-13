using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;

    public class AuditLog : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }

        public string? Action { get; set; }
        public string? EntityName { get; set; }

        public string? KeyValues { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? Changes { get; set; }

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Path { get; set; }
        public string? HttpMethod { get; set; }
    }

