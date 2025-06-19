using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Models;
using ECommerceSystem.Base.Exceptions;
using System.Text.Json;

namespace OrderService.Api.Services;

public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService
{
    private HttpClient GetClient()
    {
        return httpClientFactory.CreateClient();
    }

    public HttpRequestMessage GetPostRequest(string url, string jsonContent)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        return request;
    }

    public async Task<OrderSummary> GetJsonResponseAsync(HttpRequestMessage requestMessage)
    {
        var httpResponseMessage = await GetClient().SendAsync(requestMessage).ConfigureAwait(false);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            var orderSummary = await JsonSerializer.DeserializeAsync<OrderSummary>(contentStream).ConfigureAwait(false);
            if (orderSummary != null)
            {
                return orderSummary;
            }
        }
        throw new NotFoundException();
    }

    public async Task<HttpResponseMessage> GetHttpResponse(HttpRequestMessage requestMessage)
    {
       return await GetClient().SendAsync(requestMessage).ConfigureAwait(false);
    }

    public HttpRequestMessage GetGetRequest(string url)
    {
        return new HttpRequestMessage(HttpMethod.Get, url);
    }
}