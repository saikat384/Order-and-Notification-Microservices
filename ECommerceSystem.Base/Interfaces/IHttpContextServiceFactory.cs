using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.Base.Interfaces;

public interface IHttpContextServiceFactory
{
    IHttpContextService GetService(HttpContext httpContext);
}