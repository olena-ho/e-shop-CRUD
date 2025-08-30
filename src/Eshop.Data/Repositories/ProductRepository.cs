using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EshopDbContext _db;
    public ProductRepository(EshopDbContext db) => _db = db;

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default) =>
        _db.Products.AsNoTracking().OrderBy(p => p.Id).ToListAsync(ct);

    public Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken ct = default) =>
    _db.Products
       .AsNoTracking()
       .Where(p => p.CategoryId == categoryId)
       .OrderBy(p => p.Id)
       .ToListAsync(ct);

    public async Task<Product> AddAsync(Product entity, CancellationToken ct = default)
    {
        _db.Products.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Product entity, CancellationToken ct = default)
    {
        _db.Products.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Product entity, CancellationToken ct = default)
    {
        _db.Products.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> SkuExistsAsync(string sku, int? exceptId = null, CancellationToken ct = default) =>
        _db.Products.AnyAsync(p => p.Sku == sku && (exceptId == null || p.Id != exceptId), ct);
}
