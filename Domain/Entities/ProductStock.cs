namespace ims.Domain.Entities
{
    public class ProductStock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int WarehouseId { get; set; }

        public int Quantity { get; set; } = 0;

        public int ReservedQty { get; set; } = 0;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
