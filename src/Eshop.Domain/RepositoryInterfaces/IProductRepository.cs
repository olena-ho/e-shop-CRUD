using Eshop.Domain.Entities;

namespace Eshop.Domain.RepositoryInterfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Product>> GetAllAsync(CancellationToken ct = default);
    Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken ct = default);
    Task<Product> AddAsync(Product entity, CancellationToken ct = default);
    Task UpdateAsync(Product entity, CancellationToken ct = default);
    Task DeleteAsync(Product entity, CancellationToken ct = default);

    Task<bool> SkuExistsAsync(string sku, int? exceptId = null, CancellationToken ct = default);
}