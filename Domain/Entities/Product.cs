using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;

public class Product : BaseEntity
    {
        public new int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;

         public string? Barcode { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        public string? Unit { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal TaxRate { get; set; }

        public int LowStockAlert { get; set; }

        public string? Media { get; set; }

        public bool IsActive { get; set; } 

        public new bool IsDeleted { get; set; }

        public new DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public new DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

