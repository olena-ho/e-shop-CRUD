namespace Eshop.Application.Categories;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CategoryResponse> CreateAsync(CategoryRequestDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, CategoryRequestDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
