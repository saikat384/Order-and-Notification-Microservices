namespace ECommerceSystem.Base.Interfaces;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task Execute(TCommand command);
}