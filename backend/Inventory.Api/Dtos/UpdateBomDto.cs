namespace Inventory.Api.Dtos;

public sealed class UpdateBomDto
{
    public List<BomItemDto> Items { get; set; } = new();
}

public sealed class BomItemDto
{
    public int RawMaterialId { get; set; }
    public decimal QuantityPerUnit { get; set; }
}
