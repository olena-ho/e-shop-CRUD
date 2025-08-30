using Eshop.Domain.Entities;

namespace Eshop.Application.Orders;

public record OrderItemRequestDto(int ProductId, int Quantity);
public record OrderRequestDto(int CustomerId, List<OrderItemRequestDto> Items);

public record OrderItemResponse(int Id, int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);
public record OrderResponse(
    int Id,
    int CustomerId,
    decimal Subtotal,
    decimal Total,
    OrderStatus Status,
    DateTime CreatedUtc,
    List<OrderItemResponse> Items);
