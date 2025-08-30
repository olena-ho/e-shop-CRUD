using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo) => _repo = repo;

    public async Task<List<CustomerResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(MapToResponse).ToList();
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<CustomerResponse> CreateAsync(CustomerRequestDto dto, CancellationToken ct = default)
    {
        if (await _repo.EmailExistsAsync(dto.Email, null, ct))
            throw new InvalidOperationException("Email already exists");

        var entity = new Customer
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim().ToLowerInvariant(),
            Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim(),
        };

        var created = await _repo.AddAsync(entity, ct);
        return MapToResponse(created);
    }

    public async Task<bool> UpdateAsync(int id, CustomerRequestDto dto, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return false;

        if (await _repo.EmailExistsAsync(dto.Email, id, ct))
            throw new InvalidOperationException("Email already exists");

        existing.FirstName = dto.FirstName.Trim();
        existing.LastName = dto.LastName.Trim();
        existing.Email = dto.Email.Trim().ToLowerInvariant();
        existing.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();

        await _repo.UpdateAsync(existing, ct);
        return true;
    }

    public async Task<bool> PatchPhoneAsync(int id, string? phone, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return false;

        existing.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        await _repo.UpdateAsync(existing, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return false;

        await _repo.DeleteAsync(existing, ct);
        return true;
    }

    private static CustomerResponse MapToResponse(Customer c) =>
        new(c.Id, c.FirstName, c.LastName, c.Email, c.Phone);
}