using Eshop.Domain.Entities;

namespace Eshop.Domain.RepositoryInterfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Category>> GetAllAsync(CancellationToken ct = default);
    Task<Category> AddAsync(Category entity, CancellationToken ct = default);
    Task UpdateAsync(Category entity, CancellationToken ct = default);
    Task DeleteAsync(Category entity, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, int? exceptId = null, CancellationToken ct = default);
}
