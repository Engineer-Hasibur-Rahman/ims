namespace ims.Domain.Entities.Base;

public abstract class BaseEntity
{

    //  Why BaseEntity exists:
    //Remove duplication
    //Standard audit tracking
    //Enable soft delete
    //Maintain consistency
    //Required for ERP-level systems

    // Why abstract:
    //Prevent direct creation
    //Force inheritance only
    //Enforce design consistency

    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
