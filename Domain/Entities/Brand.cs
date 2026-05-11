namespace ims.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public string Meida { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Brand> Children { get; set; } = new List<Brand>();
    }
}
