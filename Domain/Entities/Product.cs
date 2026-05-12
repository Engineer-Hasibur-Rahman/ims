namespace ims.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }

        public string SKU { get; set; } 

        public string? Barcode { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        public string? Unit { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal TaxRate { get; set; } = 0m;

        public int LowStockAlert { get; set; } = 10;

        public string? Media { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
