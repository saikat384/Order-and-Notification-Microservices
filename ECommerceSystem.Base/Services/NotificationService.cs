using ECommerceSystem.Base.Interfaces;
using System.Text.Json;
using ECommerceSystem.Base.Models;

namespace ECommerceSystem.Base.Services;

public class NotificationService(IHttpService httpService) : INotificationService
{
    // This should be replaced with the actual URL of the notification service endpoint.
    private const string NotificationUrlEndpoint = "hosted notification endpoint url";
   
    public async Task SendNotificationAsync(OrderSummary orderSummary)
    {
        var body = JsonSerializer.Serialize(orderSummary);
        var request = httpService.GetPostRequest(NotificationUrlEndpoint, body);
        await httpService.GetHttpResponse(request).ConfigureAwait(false);
    }
}