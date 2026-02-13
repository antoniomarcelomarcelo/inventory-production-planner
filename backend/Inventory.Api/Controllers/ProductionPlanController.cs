using Inventory.Api.Dtos;
using Inventory.Api.Infrastructure;
using Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/production-plan")]
public class ProductionPlanController : ControllerBase
{
    private readonly InventoryDbContext _db;
    private readonly IProductionPlannerService _planner;

    public ProductionPlanController(InventoryDbContext db, IProductionPlannerService planner)
    {
        _db = db;
        _planner = planner;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductionPlanResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductionPlanResultDto>> Get(CancellationToken ct)
    {
        var products = await _db.Products
            .Include(p => p.BillOfMaterials)
                .ThenInclude(b => b.RawMaterial)
            .AsNoTracking()
            .ToListAsync(ct);

        var rawMaterials = await _db.RawMaterials
            .AsNoTracking()
            .ToListAsync(ct);

        var plan = _planner.GeneratePlan(products, rawMaterials);
        return Ok(plan);
    }
}
