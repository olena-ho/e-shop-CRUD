namespace Eshop.Application.Categories;

public record CategoryRequestDto(string Name, string? Description);
public record CategoryResponse(int Id, string Name, string? Description);