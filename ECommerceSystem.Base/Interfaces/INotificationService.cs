using ECommerceSystem.Base.Models;

namespace ECommerceSystem.Base.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(OrderSummary orderSummary);
}