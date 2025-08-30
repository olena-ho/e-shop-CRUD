namespace Eshop.Application.Products;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync(CancellationToken ct = default);
    Task<ProductResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<ProductResponse>> GetByCategoryAsync(int categoryId, CancellationToken ct = default);
    Task<ProductResponse> CreateAsync(ProductRequestDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, ProductRequestDto dto, CancellationToken ct = default);
    Task<bool> PatchPriceAsync(int id, decimal price, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}