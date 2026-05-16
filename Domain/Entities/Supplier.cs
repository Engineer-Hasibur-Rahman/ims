using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;

public class Supplier : BaseEntity
    {

        public string Name { get; set; } = string.Empty;

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

    }

