using Eshop.Domain.Entities;

namespace Eshop.Application.Orders;

public interface IOrderService
{
    Task<List<OrderResponse>> GetAllAsync(CancellationToken ct = default);
    Task<OrderResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<OrderResponse> CreateAsync(OrderRequestDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, OrderRequestDto dto, CancellationToken ct = default);
    Task<bool> PatchStatusAsync(int id, OrderStatus status, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
