namespace Eshop.Application.Products;

public record ProductRequestDto(string Name, string Sku, decimal Price, int CategoryId);
public record ProductResponse(int Id, string Name, string Sku, decimal Price, int CategoryId);