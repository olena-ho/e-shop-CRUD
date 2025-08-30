using Eshop.Application.Customers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CustomerRequestDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerRequestDto dto, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<CustomerRequestDto> patchDoc, CancellationToken ct)
    {
        var existing = await _service.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var dto = new CustomerRequestDto(existing.FirstName, existing.LastName, existing.Email, existing.Phone);
        patchDoc.ApplyTo(dto, ModelState);
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? Ok(id) : NotFound();
    }

    [HttpPatch("{id:int}/phone")]
    public async Task<IActionResult> PatchPhone(int id, [FromBody] string? phone, CancellationToken ct)
    {
        var ok = await _service.PatchPhoneAsync(id, phone, ct);
        return ok ? Ok($"New phone is {phone}") : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
