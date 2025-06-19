using ECommerceSystem.Base.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Domain.Commands;

public class CreateOrderCommand(OrderBaseDomainModel orderBaseDomainModel) : ICommand
{
    public OrderBaseDomainModel OrderBaseDomainModel { get; } = orderBaseDomainModel;
}