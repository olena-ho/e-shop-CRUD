using Eshop.Application.Products;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<List<ProductResponse>>> GetByCategory(int categoryId, CancellationToken ct)
    => Ok(await _service.GetByCategoryAsync(categoryId, ct));

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductRequestDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto dto, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? Ok(id) : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProductRequestDto> patchDoc, CancellationToken ct)
    {
        var existing = await _service.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var dto = new ProductRequestDto(existing.Name, existing.Sku, existing.Price, existing.CategoryId);
        patchDoc.ApplyTo(dto, ModelState);
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? Ok(id) : NotFound();
    }

    [HttpPatch("{id:int}/price")]
    public async Task<IActionResult> PatchPrice(int id, [FromBody] decimal price, CancellationToken ct)
    {
        var ok = await _service.PatchPriceAsync(id, price, ct);
        return ok ? Ok(new { id, price }) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
