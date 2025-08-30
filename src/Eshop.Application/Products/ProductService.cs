using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;

namespace Eshop.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<List<ProductResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(Map).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e is null ? null : Map(e);
    }
    public async Task<List<ProductResponse>> GetByCategoryAsync(int categoryId, CancellationToken ct = default)
    {
        var list = await _repo.GetByCategoryAsync(categoryId, ct);
        return list.Select(Map).ToList();
    }

    public async Task<ProductResponse> CreateAsync(ProductRequestDto dto, CancellationToken ct = default)
    {
        if (dto.Price < 0) throw new InvalidOperationException("Price must be non-negative.");
        if (await _repo.SkuExistsAsync(dto.Sku, null, ct))
            throw new InvalidOperationException("SKU already exists.");

        var e = new Product
        {
            Name = dto.Name.Trim(),
            Sku = dto.Sku.Trim().ToUpperInvariant(),
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        var created = await _repo.AddAsync(e, ct);
        return Map(created);
    }

    public async Task<bool> UpdateAsync(int id, ProductRequestDto dto, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null) return false;

        if (dto.Price < 0) throw new InvalidOperationException("Price must be non-negative.");
        if (await _repo.SkuExistsAsync(dto.Sku, id, ct))
            throw new InvalidOperationException("SKU already exists.");

        e.Name = dto.Name.Trim();
        e.Sku = dto.Sku.Trim().ToUpperInvariant();
        e.Price = dto.Price;
        e.CategoryId = dto.CategoryId;

        await _repo.UpdateAsync(e, ct);
        return true;
    }

    public async Task<bool> PatchPriceAsync(int id, decimal price, CancellationToken ct = default)
    {
        if (price < 0) throw new InvalidOperationException("Price must be non-negative.");
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null) return false;

        e.Price = price;
        await _repo.UpdateAsync(e, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null) return false;

        await _repo.DeleteAsync(e, ct);
        return true;
    }

    private static ProductResponse Map(Product p) =>
        new(p.Id, p.Name, p.Sku, p.Price, p.CategoryId);
}
