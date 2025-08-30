using Eshop.Application.Products;
using Eshop.Domain.Entities;
using Eshop.Domain.RepositoryInterfaces;

namespace Eshop.Application.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo) => _repo = repo;

    public async Task<List<OrderResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(o => new OrderResponse(
            o.Id, o.CustomerId, o.Subtotal, o.Total, o.Status, o.CreatedUtc, new()
        )).ToList();
    }

    public async Task<OrderResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var o = await _repo.GetByIdAsync(id, includeItems: true, ct);
        return o is null ? null : Map(o);
    }

    public async Task<OrderResponse> CreateAsync(OrderRequestDto dto, CancellationToken ct = default)
    {
        await ValidateCustomer(dto.CustomerId, ct);

        if (dto.Items is null || dto.Items.Count == 0)
            throw new InvalidOperationException("Order must contain at least one item.");
        if (dto.Items.Any(i => i.Quantity <= 0))
            throw new InvalidOperationException("Item quantity must be >= 1.");

        var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
        var priceLookup = await _repo.GetProductsUnitPricesAsync(productIds, ct);
        if (priceLookup.Count != productIds.Count)
            throw new InvalidOperationException("One or more products do not exist.");

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            Status = OrderStatus.Pending
        };

        foreach (var i in dto.Items)
        {
            var unitPrice = priceLookup[i.ProductId];
            var lineTotal = unitPrice * i.Quantity;
            order.Items.Add(new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = unitPrice,
                LineTotal = lineTotal
            });
        }

        RecalculateTotals(order);

        var created = await _repo.AddAsync(order, ct);
        var full = await _repo.GetByIdAsync(created.Id, includeItems: true, ct);
        return Map(full!);
    }

    public async Task<bool> UpdateAsync(int id, OrderRequestDto dto, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, includeItems: true, ct);
        if (existing is null) return false;

        await ValidateCustomer(dto.CustomerId, ct);
        if (dto.Items is null || dto.Items.Count == 0)
            throw new InvalidOperationException("Order must contain at least one item.");
        if (dto.Items.Any(i => i.Quantity <= 0))
            throw new InvalidOperationException("Item quantity must be >= 1.");

        existing.CustomerId = dto.CustomerId;
        existing.Items.Clear();

        var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
        var priceLookup = await _repo.GetProductsUnitPricesAsync(productIds, ct);
        if (priceLookup.Count != productIds.Count)
            throw new InvalidOperationException("One or more products do not exist.");

        foreach (var i in dto.Items)
        {
            var unitPrice = priceLookup[i.ProductId];
            existing.Items.Add(new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = unitPrice,
                LineTotal = unitPrice * i.Quantity
            });
        }

        RecalculateTotals(existing);
        await _repo.UpdateAsync(existing, ct);
        return true;
    }

    public async Task<bool> PatchStatusAsync(int id, OrderStatus status, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, includeItems: false, ct);
        if (existing is null) return false;

        existing.Status = status;
        await _repo.UpdateAsync(existing, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, includeItems: false, ct);
        if (existing is null) return false;

        await _repo.DeleteAsync(existing, ct);
        return true;
    }

    private static void RecalculateTotals(Order o)
    {
        o.Subtotal = o.Items.Sum(i => i.LineTotal);
        // place for taxes/discounts/shipping later
        o.Total = o.Subtotal;
    }

    private async Task ValidateCustomer(int customerId, CancellationToken ct)
    {
        if (!await _repo.CustomerExistsAsync(customerId, ct))
            throw new InvalidOperationException("Customer does not exist.");
    }

    private static OrderResponse Map(Order o) =>
        new(
            o.Id,
            o.CustomerId,
            o.Subtotal,
            o.Total,
            o.Status,
            o.CreatedUtc,
            o.Items.Select(i => new OrderItemResponse(
                i.Id,
                i.ProductId,
                i.Product?.Name ?? "",
                i.Quantity,
                i.UnitPrice,
                i.LineTotal)).ToList()
        );
}
