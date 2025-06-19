**Assumptions made**
1. The Database for storing orders is simulated.
2. The Command Handlers would be connecting to the DAO layer communicating with the Database.
3. Caching Service uses an in-memory caching mechanism.
4. Authentication for the endpoints is skipped.
5. While creating an order, the order status would be set to status - confirmed.
6. the NotificationController adds as an endpoint responsible to send a notifcation. The endpoint could be used to send a notification to a service.


**Architecture**
1. The OrderServiceController exposes 2 endpoints - POST endpoint to create an order and a GET enedpoint to retrieve an order by the order Id.
2. The OrderServiceController has INotificationService injected, so that the INotificationService handles and sends a notification.
3. The OrderServiceController has IOrderRepository injected, which handles retrieving the order by order Id. **(Repository pattern)**
4. The OrderServiceController has ICommandBus injected, which sends the CreateOrderCommand handled the CreateOrderCommandHandler. **(Command pattern)**
5. The OrderServiceController has IHttpContextServiceFactory injected, which creates the required httpContextService. **(Factory pattern)**
6. IValidationHandler is implemented to validate any Command and Query parameters before execution.
7. ICommandHandler is implemented to execute command execution.
8. The ValidationHandler is decorated with the respective Command so that that validation is executed first before executing the command. **(Decorator pattern)**
9. ICachingService handles the caching mechanism. the in-memory cache is cleared after an interval of 5 mins. The cache miss objects are also stored and evaluated for recently checked items to prevent redundant DB hits.
