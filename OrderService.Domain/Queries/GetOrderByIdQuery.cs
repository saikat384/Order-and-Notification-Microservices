using ECommerceSystem.Base.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Domain.Queries;

public class GetOrderByIdQuery : IQuery<OrderBaseDomainModel>
{
    public Guid OrderId { get; set; }
}