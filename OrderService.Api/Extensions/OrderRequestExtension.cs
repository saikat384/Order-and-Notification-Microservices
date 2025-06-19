using ECommerceSystem.Base.Models;
using OrderService.Domain.Models;

namespace OrderService.Api.Extensions;

public static class OrderRequestExtension
{
    public static OrderBaseDomainModel ToDomainModel(this OrderBase orderBaseRequest)
    {
        return new OrderBaseDomainModel
        {
            OrderId = Guid.NewGuid(),
            CustomerId = orderBaseRequest.CustomerId,
            ProductItemList = orderBaseRequest.ProductItemList,
            Timestamp = orderBaseRequest.Timestamp
        };
    }
}