using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EshopDbContext _db;
    public CategoryRepository(EshopDbContext db) => _db = db;

    public Task<Category?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<List<Category>> GetAllAsync(CancellationToken ct = default) =>
        _db.Categories.AsNoTracking().OrderBy(c => c.Id).ToListAsync(ct);

    public async Task<Category> AddAsync(Category entity, CancellationToken ct = default)
    {
        _db.Categories.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Category entity, CancellationToken ct = default)
    {
        _db.Categories.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Category entity, CancellationToken ct = default)
    {
        _db.Categories.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> NameExistsAsync(string name, int? exceptId = null, CancellationToken ct = default) =>
        _db.Categories.AnyAsync(c => c.Name == name && (exceptId == null || c.Id != exceptId), ct);
}
