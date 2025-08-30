namespace Eshop.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }

    // navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
