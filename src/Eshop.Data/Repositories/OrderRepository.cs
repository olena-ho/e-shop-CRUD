using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly EshopDbContext _db;
    public OrderRepository(EshopDbContext db) => _db = db;

    public async Task<Order?> GetByIdAsync(int id, bool includeItems = false, CancellationToken ct = default)
    {
        IQueryable<Order> q = _db.Orders.AsNoTracking();
        if (includeItems)
            q = q.Include(o => o.Items).ThenInclude(i => i.Product);
        return await q.FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public Task<List<Order>> GetAllAsync(CancellationToken ct = default) =>
        _db.Orders.AsNoTracking()
                  .OrderByDescending(o => o.Id)
                  .ToListAsync(ct);

    public async Task<Order> AddAsync(Order entity, CancellationToken ct = default)
    {
        _db.Orders.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Order entity, CancellationToken ct = default)
    {
        _db.Orders.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Order entity, CancellationToken ct = default)
    {
        _db.Orders.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> CustomerExistsAsync(int customerId, CancellationToken ct = default) =>
        _db.Customers.AnyAsync(c => c.Id == customerId, ct);

    public async Task<Dictionary<int, decimal>> GetProductsUnitPricesAsync(IEnumerable<int> productIds, CancellationToken ct = default)
    {
        return await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Price })
            .ToDictionaryAsync(x => x.Id, x => x.Price, ct);
    }
}
