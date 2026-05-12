namespace ims.Domain.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Location { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }     
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
