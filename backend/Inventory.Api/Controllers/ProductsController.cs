using Inventory.Api.Domain.Entities;
using Inventory.Api.Dtos;
using Inventory.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly InventoryDbContext _db;

    public ProductsController(InventoryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _db.Products
            .Include(p => p.BillOfMaterials)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _db.Products
            .Include(p => p.BillOfMaterials)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product updated)
    {
        if (id != updated.Id)
            return BadRequest();

        _db.Entry(updated).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}/bom")]
    public async Task<IActionResult> UpdateBom(int id, UpdateBomDto dto)
    {
        var product = await _db.Products
            .Include(p => p.BillOfMaterials)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        // Basic validation
        if (dto.Items.Any(x => x.QuantityPerUnit <= 0))
            return BadRequest("QuantityPerUnit must be greater than 0.");

        // remove old BOM items for this product
        _db.BillOfMaterialItems.RemoveRange(product.BillOfMaterials);

        // add new items
        foreach (var item in dto.Items)
        {
            product.BillOfMaterials.Add(new BillOfMaterialItem
            {
                ProductId = id,
                RawMaterialId = item.RawMaterialId,
                QuantityPerUnit = item.QuantityPerUnit
            });
        }

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/bom")]
    public async Task<IActionResult> GetBom(int id)
    {
        var items = await _db.BillOfMaterialItems
            .Where(x => x.ProductId == id)
            .Select(x => new
            {
                x.RawMaterialId,
                x.QuantityPerUnit
            })
            .ToListAsync();

        return Ok(new { items });
    }

}
