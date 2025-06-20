using ECommerceSystem.Base.Enums;
using ECommerceSystem.Base.Exceptions;
using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderService.Api.Controllers;
using OrderService.Domain.Commands;
using System.Diagnostics.CodeAnalysis;

namespace OrderService.Api.Test.Controllers;

[TestClass]
[ExcludeFromCodeCoverage]
public class OrderServiceControllerTest
{
    private const string DisplayUrl = nameof(DisplayUrl);
    private Mock<IHttpContextService>? _httpContextServiceMock;
    private Mock<IHttpContextServiceFactory>? _httpContextServiceFactoryMock;
    private Mock<ICommandBus>? _commandBusMock;
    private Mock<INotificationService>? _notificationServiceMock;
    private Mock<IOrderRepository>? _orderRepositoryMock;

    private OrderServiceController? _sut;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextServiceMock = new Mock<IHttpContextService>(MockBehavior.Strict);
        _httpContextServiceMock.Setup(x => x.GetDisplayUrl()).Returns(DisplayUrl);

        _httpContextServiceFactoryMock = new Mock<IHttpContextServiceFactory>(MockBehavior.Strict);

        _commandBusMock = new Mock<ICommandBus>(MockBehavior.Strict);
        _notificationServiceMock = new Mock<INotificationService>(MockBehavior.Strict);
        _orderRepositoryMock = new Mock<IOrderRepository>(MockBehavior.Strict);
        _httpContextServiceFactoryMock.Setup(x => x.GetService(It.IsAny<HttpContext>()))
            .Returns(_httpContextServiceMock.Object);

        _sut = new OrderServiceController(_httpContextServiceFactoryMock.Object, _commandBusMock.Object,
            _notificationServiceMock.Object, _orderRepositoryMock.Object);
    }

    [TestMethod]
    public async Task PostOrders_OrderCreated_ReturnsCreatedResult()
    {
        // Arrange
        _commandBusMock!.Setup(x => x.Send(It.IsAny<IHttpContextService>(), It.IsAny<CreateOrderCommand>()))
            .Returns(Task.CompletedTask);

        _notificationServiceMock!.Setup(x => x.SendNotificationAsync(It.IsAny<OrderSummary>()))
            .Returns(Task.CompletedTask);

        var productItemList = new List<ProductItemDetails>
        {
            new() {ProductId = 1, Quantity = 2},
            new() {ProductId = 2, Quantity = 3}
        };
        var createOrderRequest = new OrderBase
            { CustomerId = 23, ProductItemList = productItemList, Timestamp = DateTime.UtcNow };

        // Act
        var result = await _sut!.Orders(createOrderRequest).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(CreatedResult));

        _commandBusMock.Verify(x => x.Send(It.IsAny<IHttpContextService>(), It.IsAny<CreateOrderCommand>()), Times.Once);
        _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.IsAny<OrderSummary>()), Times.Once);
    }

    [TestMethod]
    public async Task PostOrders_CommandBusThrowsException_ReturnsBadRequestAndNotificationIsNotSent()
    {
        // Arrange
        _commandBusMock!.Setup(x => x.Send(It.IsAny<IHttpContextService>(), It.IsAny<CreateOrderCommand>()))
            .Throws(new Exception());

        _notificationServiceMock!.Setup(x => x.SendNotificationAsync(It.IsAny<OrderSummary>()))
            .Returns(Task.CompletedTask);

        var productItemList = new List<ProductItemDetails>
        {
            new() {ProductId = 1, Quantity = 2},
            new() {ProductId = 2, Quantity = 3}
        };
        var createOrderRequest = new OrderBase
            { CustomerId = 23, ProductItemList = productItemList, Timestamp = DateTime.UtcNow };

        // Act
        var result = await _sut!.Orders(createOrderRequest).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        _commandBusMock.Verify(x => x.Send(It.IsAny<IHttpContextService>(), It.IsAny<CreateOrderCommand>()), Times.Once);
        _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.IsAny<OrderSummary>()), Times.Never);
    }

    [TestMethod]
    public async Task GetOrders_OrderSummaryAvailable_ReturnsOkResult()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var productItemList = new List<ProductItemDetails>
        {
            new() {ProductId = 1, Quantity = 2},
            new() {ProductId = 2, Quantity = 3}
        };
        var orderBase = new OrderBase
            { CustomerId = 23, ProductItemList = productItemList, Timestamp = DateTime.UtcNow };

        _orderRepositoryMock!.Setup(x => x.GetOrderAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new OrderSummary(orderBase, orderId, OrderStatus.Shipped));

        // Act
        var result = await _sut!.Orders(orderId).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        _orderRepositoryMock.Verify(x => x.GetOrderAsync(It.Is<Guid>(guid=>guid.Equals(orderId))), Times.Once);
    }

    [TestMethod]
    public async Task GetOrders_OrderNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock!.Setup(x => x.GetOrderAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new NotFoundException());

        // Act
        var result = await _sut!.Orders(orderId).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        _orderRepositoryMock.Verify(x => x.GetOrderAsync(It.Is<Guid>(guid => guid.Equals(orderId))), Times.Once);
    }

    [TestMethod]
    public async Task GetOrders_OrderRepositoryThrowsException_BadRequestResult()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock!.Setup(x => x.GetOrderAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _sut!.Orders(orderId).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        _orderRepositoryMock.Verify(x => x.GetOrderAsync(It.Is<Guid>(guid => guid.Equals(orderId))), Times.Once);
    }
}