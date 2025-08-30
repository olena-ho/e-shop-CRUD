namespace Eshop.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Sku { get; set; }

    public decimal Price { get; set; }

    // FK
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

}

