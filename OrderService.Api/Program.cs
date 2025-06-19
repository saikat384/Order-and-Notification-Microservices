using ECommerceSystem.Base.Decorators;
using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Repositories;
using ECommerceSystem.Base.Services;
using OrderService.Api.Extensions;
using OrderService.Api.Services;
using OrderService.Domain.Validators;
using OrderService.Service;
using OrderService.Service.CommandHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ICachingService, CachingService>();
builder.Services.AddSingleton<IHttpContextServiceFactory, HttpContextServiceFactory>();
builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<ICommandBus, CommandBus>();

builder.Services.AddTransientFromAssembly(typeof(ICommandHandler<>), typeof(CreateOrderCommandHandler).Assembly);
builder.Services.AddTransientFromAssembly(typeof(IValidationHandler<>), typeof(CreateOrderCommandValidator).Assembly);

builder.Services.AddDecorator(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));
builder.Services.AddDecorator(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
