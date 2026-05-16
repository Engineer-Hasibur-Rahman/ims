using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;

    public class Brand : BaseEntity
    {
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Meida { get; set; } = string.Empty;
    public int? ParentId { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Brand> Children { get; set; } = new List<Brand>();
    }

