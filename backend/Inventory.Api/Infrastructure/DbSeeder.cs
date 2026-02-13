using Inventory.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(InventoryDbContext db, CancellationToken ct = default)
    {
        // Apply migrations automatically
        await db.Database.MigrateAsync(ct);

        if (await db.Products.AnyAsync(ct))
            return;

        // Raw Materials
        var steel = new RawMaterial { Code = "RM-STEEL", Name = "Steel", StockQuantity = 10m };
        var plastic = new RawMaterial { Code = "RM-PLASTIC", Name = "Plastic", StockQuantity = 100m };
        var paint = new RawMaterial { Code = "RM-PAINT", Name = "Paint", StockQuantity = 20m };

        db.RawMaterials.AddRange(steel, plastic, paint);
        await db.SaveChangesAsync(ct);

        // Products (A is higher value than B and shares Steel => proves priority)
        var productA = new Product { Code = "P-A", Name = "Premium Product", Price = 200m };
        var productB = new Product { Code = "P-B", Name = "Standard Product", Price = 80m };

        db.Products.AddRange(productA, productB);
        await db.SaveChangesAsync(ct);

        // BOM
        db.BillOfMaterialItems.AddRange(
            new BillOfMaterialItem { ProductId = productA.Id, RawMaterialId = steel.Id, QuantityPerUnit = 4m },
            new BillOfMaterialItem { ProductId = productA.Id, RawMaterialId = paint.Id, QuantityPerUnit = 1m },

            new BillOfMaterialItem { ProductId = productB.Id, RawMaterialId = steel.Id, QuantityPerUnit = 2m },
            new BillOfMaterialItem { ProductId = productB.Id, RawMaterialId = plastic.Id, QuantityPerUnit = 10m }
        );

        await db.SaveChangesAsync(ct);
    }
}
