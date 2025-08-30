using Eshop.Domain.Entities;

namespace Eshop.Domain.RepositoryInterfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, bool includeItems = false, CancellationToken ct = default);
    Task<List<Order>> GetAllAsync(CancellationToken ct = default);
    Task<Order> AddAsync(Order entity, CancellationToken ct = default);
    Task UpdateAsync(Order entity, CancellationToken ct = default);
    Task DeleteAsync(Order entity, CancellationToken ct = default);

    Task<bool> CustomerExistsAsync(int customerId, CancellationToken ct = default);
    Task<Dictionary<int, decimal>> GetProductsUnitPricesAsync(IEnumerable<int> productIds, CancellationToken ct = default);
}
