using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ims.Domain.Entities.Base;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public Guid StringId { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
