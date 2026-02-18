using Inventory.Api.Domain.Entities;
using Inventory.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/raw-materials")]
public class RawMaterialsController : ControllerBase
{
    private readonly InventoryDbContext _db;

    public RawMaterialsController(InventoryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RawMaterial>>> GetAll()
    {
        return Ok(await _db.RawMaterials.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RawMaterial>> GetById(int id)
    {
        var rm = await _db.RawMaterials.FindAsync(id);

        if (rm == null)
            return NotFound();

        return Ok(rm);
    }

    [HttpPost]
    public async Task<ActionResult<RawMaterial>> Create(RawMaterial rawMaterial)
    {
        _db.RawMaterials.Add(rawMaterial);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = rawMaterial.Id }, rawMaterial);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RawMaterial updated)
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
        var rm = await _db.RawMaterials.FindAsync(id);

        if (rm == null)
            return NotFound();

        _db.RawMaterials.Remove(rm);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
