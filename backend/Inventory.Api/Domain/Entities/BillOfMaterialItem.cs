namespace Inventory.Api.Domain.Entities
{
    public class BillOfMaterialItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int RawMaterialId { get; set; }
        public RawMaterial RawMaterial { get; set; } = default!;

        // quantity of raw material needed to produce 1 unit of product
        public decimal QuantityPerUnit { get; set; }
    }
}
