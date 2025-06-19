using ECommerceSystem.Base.Enums;
using ECommerceSystem.Base.Exceptions;
using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Models;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Extensions;
using OrderService.Domain.Commands;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderServiceController(IHttpContextServiceFactory httpContextServiceFactory, ICommandBus commandBus, 
    INotificationService notificationService, IOrderRepository orderRepository) : ControllerBase
{
    [HttpPost(nameof(Orders))]
    public async Task<IActionResult> Orders([FromBody] OrderBase createOrderRequest)
    {
        try
        {
            var httpContextService = httpContextServiceFactory.GetService(HttpContext);
            var domainModel = createOrderRequest.ToDomainModel();

            await commandBus.Send(httpContextService, new CreateOrderCommand(domainModel)).ConfigureAwait(false);

            var orderSummary = new OrderSummary(createOrderRequest, domainModel.OrderId, OrderStatus.Confirmed);

            _ = notificationService.SendNotificationAsync(orderSummary).ConfigureAwait(false);

            return new CreatedResult(httpContextService.GetDisplayUrl(), orderSummary);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet(nameof(Orders))]
    public async Task<IActionResult> Orders(Guid id)
    {
        try
        {
            var order = await orderRepository.GetOrderAsync(id).ConfigureAwait(false);
            return Ok(order);
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}