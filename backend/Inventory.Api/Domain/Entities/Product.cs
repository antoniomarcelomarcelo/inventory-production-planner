namespace Inventory.Api.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }

        public List<BillOfMaterialItem> BillOfMaterials { get; set; } = new();
    }
}
