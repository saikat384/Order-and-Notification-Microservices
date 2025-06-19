using ECommerceSystem.Base.Interfaces;

namespace ECommerceSystem.Base.Decorators;

public class ValidationQueryHandlerDecorator<TQuery, TResult>(IQueryHandler<TQuery, TResult> decoratedQueryHandler,
    IValidationHandler<TQuery> validationHandler) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private IQueryHandler<TQuery, TResult> Decorated { get; } = decoratedQueryHandler;

    private IValidationHandler<TQuery> ValidationHandler { get; } = validationHandler;

    public async Task<TResult> Handle(TQuery query)
    {
        await ValidationHandler.Validate(query).ConfigureAwait(false);
        return await Decorated.Handle(query).ConfigureAwait(false);
    }
}