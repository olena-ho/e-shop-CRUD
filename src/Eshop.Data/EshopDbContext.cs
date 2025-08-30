using Eshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data;

public class EshopDbContext : DbContext
{
    public EshopDbContext(DbContextOptions<EshopDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(cfg =>
        {
            cfg.ToTable("categories");
            cfg.HasKey(x => x.Id);

            cfg.Property(x => x.Name).IsRequired().HasMaxLength(100);
            cfg.Property(x => x.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Product>(cfg =>
        {
            cfg.ToTable("products");
            cfg.HasKey(x => x.Id);

            cfg.Property(x => x.Name).IsRequired().HasMaxLength(200);
            cfg.Property(x => x.Sku).IsRequired().HasMaxLength(64);
            cfg.HasIndex(x => x.Sku).IsUnique();
            cfg.Property(x => x.Price).HasPrecision(18, 2).IsRequired();

            cfg.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Customer>(cfg =>
        {
            cfg.ToTable("customers");
            cfg.HasKey(x => x.Id);
            cfg.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            cfg.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            cfg.Property(x => x.Email).IsRequired().HasMaxLength(256);
            cfg.HasIndex(x => x.Email).IsUnique();
            cfg.Property(x => x.Phone).HasMaxLength(32);
        });

        modelBuilder.Entity<Order>(cfg =>
        {
            cfg.ToTable("orders");
            cfg.HasKey(x => x.Id);

            cfg.Property(x => x.Subtotal).HasPrecision(18, 2).IsRequired();
            cfg.Property(x => x.Total).HasPrecision(18, 2).IsRequired();

            cfg.Property(x => x.Status).HasConversion<int>().IsRequired();

            cfg.HasOne(o => o.Customer)
               .WithMany()                         // or .WithMany(c => c.Orders) if you add navigation on Customer
               .HasForeignKey(o => o.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(cfg =>
        {
            cfg.ToTable("order_items");
            cfg.HasKey(x => x.Id);

            cfg.Property(x => x.UnitPrice).HasPrecision(18, 2).IsRequired();
            cfg.Property(x => x.LineTotal).HasPrecision(18, 2).IsRequired();
            cfg.Property(x => x.Quantity).IsRequired();

            cfg.HasOne(oi => oi.Order)
               .WithMany(o => o.Items)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            cfg.HasOne(oi => oi.Product)
               .WithMany()
               .HasForeignKey(oi => oi.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
