using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly EshopDbContext _db;

    public CustomerRepository(EshopDbContext db) => _db = db;

    public Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<Customer>> GetAllAsync(CancellationToken ct = default) =>
        _db.Customers.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);

    public async Task<Customer> AddAsync(Customer entity, CancellationToken ct = default)
    {
        _db.Customers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Customer entity, CancellationToken ct = default)
    {
        _db.Customers.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Customer entity, CancellationToken ct = default)
    {
        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> EmailExistsAsync(string email, int? exceptId = null, CancellationToken ct = default) =>
        _db.Customers.AnyAsync(c => c.Email == email && (exceptId == null || c.Id != exceptId), ct);
}