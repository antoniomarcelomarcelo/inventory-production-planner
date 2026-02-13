using FluentAssertions;
using Inventory.Api.Domain.Entities;
using Inventory.Api.Services;
using Xunit;

namespace Inventory.Tests;

public class ProductionPlannerServiceTests
{
    [Fact]
    public void GeneratePlan_ShouldPrioritizeHigherPrice_AndConsumeStock()
    {
        // Arrange
        var steel = new RawMaterial { Id = 1, Code = "RM-STEEL", Name = "Steel", StockQuantity = 10m };
        var plastic = new RawMaterial { Id = 2, Code = "RM-PLASTIC", Name = "Plastic", StockQuantity = 100m };
        var paint = new RawMaterial { Id = 3, Code = "RM-PAINT", Name = "Paint", StockQuantity = 20m };

        var premium = new Product { Id = 1, Code = "P-A", Name = "Premium Product", Price = 200m };
        var standard = new Product { Id = 2, Code = "P-B", Name = "Standard Product", Price = 80m };

        // Premium uses Steel 4 and Paint 1 per unit => with Steel=10 => 2 units (bottleneck Steel)
        premium.BillOfMaterials.AddRange(new[]
        {
            new BillOfMaterialItem { ProductId = premium.Id, RawMaterialId = steel.Id, QuantityPerUnit = 4m },
            new BillOfMaterialItem { ProductId = premium.Id, RawMaterialId = paint.Id, QuantityPerUnit = 1m },
        });

        // Standard uses Steel 2 and Plastic 10 per unit
        // After producing Premium (2 units), Steel left = 2 => Standard => 1 unit
        standard.BillOfMaterials.AddRange(new[]
        {
            new BillOfMaterialItem { ProductId = standard.Id, RawMaterialId = steel.Id, QuantityPerUnit = 2m },
            new BillOfMaterialItem { ProductId = standard.Id, RawMaterialId = plastic.Id, QuantityPerUnit = 10m },
        });

        var planner = new ProductionPlannerService();

        // Act
        var result = planner.GeneratePlan(
            products: new[] { standard, premium }, // intentionally unsorted
            rawMaterials: new[] { steel, plastic, paint });

        // Assert
        result.Items.Should().HaveCount(2);

        // Must prioritize higher price first
        result.Items[0].ProductCode.Should().Be("P-A");
        result.Items[0].ProducibleQuantity.Should().Be(2);
        result.Items[0].TotalValue.Should().Be(400m);

        result.Items[1].ProductCode.Should().Be("P-B");
        result.Items[1].ProducibleQuantity.Should().Be(1);
        result.Items[1].TotalValue.Should().Be(80m);

        result.GrandTotalValue.Should().Be(480m);

        // Nice-to-have: bottleneck should be Steel for both in this scenario
        result.Items[0].BottleneckRawMaterial.Should().Be("Steel");
        result.Items[1].BottleneckRawMaterial.Should().Be("Steel");
    }

    [Fact]
    public void GeneratePlan_ShouldSkipProductsWithoutBom()
    {
        // Arrange
        var rm = new RawMaterial { Id = 1, Code = "RM-1", Name = "Any", StockQuantity = 10m };
        var product = new Product { Id = 1, Code = "P-1", Name = "No BOM", Price = 999m };

        var planner = new ProductionPlannerService();

        // Act
        var result = planner.GeneratePlan(new[] { product }, new[] { rm });

        // Assert
        result.Items.Should().BeEmpty();
        result.GrandTotalValue.Should().Be(0m);
    }
}
