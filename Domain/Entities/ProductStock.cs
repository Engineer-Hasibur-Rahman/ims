using ims.Domain.Entities.Base;

namespace ims.Domain.Entities;

    public class ProductStock : BaseEntity
{

        public int ProductId { get; set; }

        public int WarehouseId { get; set; }

        public int Quantity { get; set; } = 0;

        public int ReservedQty { get; set; } = 0;
    }

