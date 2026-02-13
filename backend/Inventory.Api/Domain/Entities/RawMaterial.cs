namespace Inventory.Api.Domain.Entities
{
    public class RawMaterial
    {
        public int Id { get; set; }

        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;

        // available stock for production
        public decimal StockQuantity { get; set; }

        public List<BillOfMaterialItem> BillOfMaterials { get; set; } = new();
    }
}
