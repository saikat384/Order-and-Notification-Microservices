namespace ECommerceSystem.Base.Interfaces;

public interface IValidationHandler<in TQuery>
{
    Task Validate(TQuery query);
}