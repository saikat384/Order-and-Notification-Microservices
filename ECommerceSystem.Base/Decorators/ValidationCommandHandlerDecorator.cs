using ECommerceSystem.Base.Interfaces;

namespace ECommerceSystem.Base.Decorators;

public class ValidationCommandHandlerDecorator<TCommand>(ICommandHandler<TCommand> decoratedCommandHandler,
    IValidationHandler<TCommand> validationHandler) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private ICommandHandler<TCommand> Decorated { get; } = decoratedCommandHandler;
    private IValidationHandler<TCommand> ValidationHandler { get; } = validationHandler;

    public async Task Execute( TCommand command)
    {
        await ValidationHandler.Validate(command).ConfigureAwait(false);
        await Decorated.Execute(command).ConfigureAwait(false);
    }
}