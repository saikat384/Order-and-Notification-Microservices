using ECommerceSystem.Base.Models;

namespace ECommerceSystem.Base.Interfaces;

public interface ICachingService
{
    Task<OrderSummary> GetOrder(Guid orderId);
}