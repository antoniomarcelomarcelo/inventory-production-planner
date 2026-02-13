using Inventory.Api.Domain.Entities;
using Inventory.Api.Dtos;

namespace Inventory.Api.Services;

public interface IProductionPlannerService
{
    ProductionPlanResultDto GeneratePlan(
        IReadOnlyList<Product> products,
        IReadOnlyList<RawMaterial> rawMaterials);
}

public sealed class ProductionPlannerService : IProductionPlannerService
{
    public ProductionPlanResultDto GeneratePlan(
        IReadOnlyList<Product> products,
        IReadOnlyList<RawMaterial> rawMaterials)
    {
        // work on a copy of stock so we can "consume" during planning
        var stock = rawMaterials.ToDictionary(r => r.Id, r => r.StockQuantity);
        var rawMaterialName = rawMaterials.ToDictionary(r => r.Id, r => r.Name);

        var result = new ProductionPlanResultDto();

        foreach (var product in products.OrderByDescending(p => p.Price))
        {
            if (product.BillOfMaterials is null || product.BillOfMaterials.Count == 0)
                continue;

            // if any required raw material is missing in stock map, treat as 0
            decimal maxPossible = decimal.MaxValue;
            string? bottleneck = null;

            foreach (var item in product.BillOfMaterials)
            {
                if (item.QuantityPerUnit <= 0)
                    continue; // validation can be enforced in API too

                stock.TryGetValue(item.RawMaterialId, out var available);
                var possible = Math.Floor((double)(available / item.QuantityPerUnit));

                if ((decimal)possible < maxPossible)
                {
                    maxPossible = (decimal)possible;
                    bottleneck = rawMaterialName.TryGetValue(item.RawMaterialId, out var n) ? n : null;
                }
            }

            var qty = (int)Math.Max(0, maxPossible);
            if (qty == 0) continue;

            // consume stock
            foreach (var item in product.BillOfMaterials)
            {
                if (item.QuantityPerUnit <= 0) continue;

                var consumed = qty * item.QuantityPerUnit;
                stock[item.RawMaterialId] = (stock.TryGetValue(item.RawMaterialId, out var s) ? s : 0m) - consumed;
            }

            var totalValue = qty * product.Price;

            result.Items.Add(new ProductionPlanItemDto
            {
                ProductId = product.Id,
                ProductCode = product.Code,
                ProductName = product.Name,
                UnitPrice = product.Price,
                ProducibleQuantity = qty,
                TotalValue = totalValue,
                BottleneckRawMaterial = bottleneck
            });

            result.GrandTotalValue += totalValue;
        }

        return result;
    }
}
