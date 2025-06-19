using ECommerceSystem.Base.Enums;

namespace ECommerceSystem.Base.Models;

public class OrderSummary: OrderBase
{
    public OrderSummary(OrderBase orderBase, Guid orderId, OrderStatus status)
    {
        CustomerId = orderBase.CustomerId;
        ProductItemList = orderBase.ProductItemList;
        Timestamp = orderBase.Timestamp;
        OrderId = orderId;
        Status = status;
    }

    public Guid OrderId { get; }
    public OrderStatus Status { get;}
}