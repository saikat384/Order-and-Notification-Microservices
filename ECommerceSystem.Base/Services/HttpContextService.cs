using ECommerceSystem.Base.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceSystem.Base.Services;

public class HttpContextService(HttpContext httpContext) : IHttpContextService
{
    public TService GetRequiredService<TService>() where TService : notnull
    {
        return httpContext.RequestServices.GetRequiredService<TService>();
    }

    public string GetDisplayUrl()
    {
        return httpContext.Request.GetDisplayUrl();
    }
}