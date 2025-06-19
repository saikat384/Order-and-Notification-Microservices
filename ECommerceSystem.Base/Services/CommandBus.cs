using ECommerceSystem.Base.Interfaces;

namespace ECommerceSystem.Base.Services;

public class CommandBus : ICommandBus
{
    public async Task Send<T>(IHttpContextService httpContextService, T command) where T : ICommand
    {
        var handler = httpContextService.GetRequiredService<ICommandHandler<T>>();

        await handler.Execute(command).ConfigureAwait(false);
    }
}