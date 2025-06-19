using ECommerceSystem.Base.Interfaces;
using OrderService.Domain.Commands;

namespace OrderService.Service.CommandHandlers;

public class CreateOrderCommandHandler: ICommandHandler<CreateOrderCommand>
{
    public async Task Execute(CreateOrderCommand command)
    {
        //Connects to the Dao layer to add entry in the DB
        // Simulating database operation with a delay
        await Task.Delay(1000).ConfigureAwait(false);
    }
}