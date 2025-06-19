using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Services;
using Microsoft.AspNetCore.Http;

namespace OrderService.Service;

public class HttpContextServiceFactory : IHttpContextServiceFactory
{
    public IHttpContextService GetService(HttpContext httpContext)
    {
        return new HttpContextService(httpContext);
    }
}