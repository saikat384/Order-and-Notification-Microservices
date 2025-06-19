using ECommerceSystem.Base.Models;

namespace ECommerceSystem.Base.Interfaces;

public interface IOrderRepository
{
    Task<OrderSummary> GetOrderAsync(Guid orderId);
}