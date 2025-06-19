using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Services;
using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.Base.Factories;

public class HttpContextServiceFactory : IHttpContextServiceFactory
{
    public IHttpContextService GetService(HttpContext httpContext)
    {
        return new HttpContextService(httpContext);
    }
}