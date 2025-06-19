using ECommerceSystem.Base.Models;

namespace ECommerceSystem.Base.Interfaces;

public interface IHttpService
{
    HttpRequestMessage GetPostRequest(string url, string jsonContent);

    Task<OrderSummary> GetJsonResponseAsync(HttpRequestMessage requestMessage);

    Task<HttpResponseMessage> GetHttpResponse(HttpRequestMessage requestMessage);

}