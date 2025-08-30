using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Eshop.Data.Repositories;
using Eshop.Domain.RepositoryInterfaces;


namespace Eshop.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("EshopDb");

        if (string.IsNullOrWhiteSpace(cs))
        {
            throw new InvalidOperationException("ConnectionStrings:EshopDb is not configured. Set it via User Secrets or environment variable.");
        }

        services.AddDbContext<EshopDbContext>(opt => opt.UseNpgsql(cs));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
