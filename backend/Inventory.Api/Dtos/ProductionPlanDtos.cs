namespace Inventory.Api.Dtos
{
    public sealed class ProductionPlanResultDto
    {
        public List<ProductionPlanItemDto> Items { get; set; } = new();
        public decimal GrandTotalValue { get; set; }
    }

    public sealed class ProductionPlanItemDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public decimal UnitPrice { get; set; }

        public int ProducibleQuantity { get; set; }
        public decimal TotalValue { get; set; }

        // nice-to-have (impressiona)
        public string? BottleneckRawMaterial { get; set; }
    }
}
