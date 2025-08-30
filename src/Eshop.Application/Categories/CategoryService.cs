using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;

namespace Eshop.Application.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    public CategoryService(ICategoryRepository repo) => _repo = repo;

    public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(Map).ToList();
    }

    public async Task<CategoryResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e is null ? null : Map(e);
    }

    public async Task<CategoryResponse> CreateAsync(CategoryRequestDto dto, CancellationToken ct = default)
    {
        if (await _repo.NameExistsAsync(dto.Name, null, ct))
            throw new InvalidOperationException("Category name already exists.");

        var e = new Category
        {
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim()
        };

        var created = await _repo.AddAsync(e, ct);
        return Map(created);
    }

    public async Task<bool> UpdateAsync(int id, CategoryRequestDto dto, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null) return false;

        if (await _repo.NameExistsAsync(dto.Name, id, ct))
            throw new InvalidOperationException("Category name already exists.");

        e.Name = dto.Name.Trim();
        e.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

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

    private static CategoryResponse Map(Category c) =>
        new(c.Id, c.Name, c.Description);
}
