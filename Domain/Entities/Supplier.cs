namespace ims.Domain.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? ContactName{ get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }       
        public string? Media { get; set; }       

        public int? CountryId { get; set; }

        public int? StateId { get; set; }

        public int? CityId { get; set; }

        public int? AreaId { get; set; }

        public string? Address { get; set; }

        public string? TaxNumber { get; set; }

        public decimal OpeningBalance { get; set; } = 0m;

        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
