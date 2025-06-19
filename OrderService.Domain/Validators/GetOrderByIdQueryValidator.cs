using ECommerceSystem.Base.Interfaces;
using OrderService.Domain.Queries;

namespace OrderService.Domain.Validators;

public class GetOrderByIdQueryValidator : IValidationHandler<GetOrderByIdQuery>
{
    public Task Validate(GetOrderByIdQuery query)
    {
        if (query.OrderId == Guid.Empty)
        {
            throw new ArgumentOutOfRangeException(nameof(query.OrderId));

        }
        return Task.CompletedTask;
    }
}