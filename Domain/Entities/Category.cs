
using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;
    public class Category : BaseEntity
    {
   

    public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Media { get; set; } = string.Empty;
        public int? ParentId { get; set; }      
        public bool IsActive { get; set; }

        // Recursive Relationship
        public Category? Parent { get; set; }
        public ICollection<Category> Children { get; set; }
            = new List<Category>();
    }

