using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; } // navigation (may not be loaded)

    // totals are computed from items (service will maintain these)
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
