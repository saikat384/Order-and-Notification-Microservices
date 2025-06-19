namespace ECommerceSystem.Base.Interfaces;

public interface IHttpContextService
{
    TService GetRequiredService<TService>() where TService : notnull;
    string GetDisplayUrl();
}