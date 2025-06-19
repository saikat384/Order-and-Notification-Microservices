using ECommerceSystem.Base.Interfaces;
using OrderService.Domain.Commands;

namespace OrderService.Domain.Validators;

public class CreateOrderCommandValidator : IValidationHandler<CreateOrderCommand>
{
    public Task Validate(CreateOrderCommand command)
    {
        if (command.OrderBaseDomainModel.CustomerId <= 0)
        {
            throw new ArgumentException("CustomerId must be greater than zero.", nameof(command.OrderBaseDomainModel.CustomerId));
        }

        if (command.OrderBaseDomainModel.ProductItemList == null)
        {
            throw new ArgumentNullException(nameof(command.OrderBaseDomainModel.ProductItemList), "ProductItemList cannot be null.");
        }

        if (command.OrderBaseDomainModel.ProductItemList.Count == 0)
        {
            throw new ArgumentException("ProductItemList cannot be empty.", nameof(command.OrderBaseDomainModel.ProductItemList));
        }

        foreach (var item in command.OrderBaseDomainModel.ProductItemList)
        {
            if (item.ProductId <= 0)
            {
                throw new ArgumentException("ProductId must be greater than zero.", nameof(item.ProductId));
            }
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(item.Quantity));
            }
        }
        return Task.CompletedTask;
    }
}