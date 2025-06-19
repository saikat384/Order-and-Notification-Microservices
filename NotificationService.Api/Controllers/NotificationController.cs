using ECommerceSystem.Base.Models;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    /// <summary>
    /// Mock endpoint for sending notifications.
    /// </summary>
    /// <param name="orderSummary"></param>
    /// <returns></returns>
    [HttpPost(nameof(SendNotification))]
    public async Task<IActionResult> SendNotification([FromBody] OrderSummary orderSummary)
    {
        await Task.Delay(1000);
        // Performs operation for sending a notification
        return Ok();
    }
}
