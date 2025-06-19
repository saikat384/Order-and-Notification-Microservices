namespace ECommerceSystem.Base.Interfaces;

public interface ICommandBus
{
    Task Send<T>(IHttpContextService httpContextService, T command) where T : ICommand;
}