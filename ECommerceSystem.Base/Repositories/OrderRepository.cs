using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Models;

namespace ECommerceSystem.Base.Repositories;

public class OrderRepository(ICachingService cachingService) : IOrderRepository
{
    public Task<OrderSummary> GetOrderAsync(Guid orderId)
    {
        return cachingService.GetOrder(orderId);
    }
}