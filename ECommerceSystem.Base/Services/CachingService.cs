using ECommerceSystem.Base.Enums;
using ECommerceSystem.Base.Exceptions;
using ECommerceSystem.Base.Interfaces;
using ECommerceSystem.Base.Models;
using System.Collections.Concurrent;
using System.Timers;

namespace ECommerceSystem.Base.Services;

public class CachingService : ICachingService
{
    private readonly IDictionary<Guid, OrderSummary> _orderSummaryCache = new ConcurrentDictionary<Guid, OrderSummary>();
    private readonly IDictionary<Guid, OrderSummaryCacheMiss> _missingOrderSummaries = new ConcurrentDictionary<Guid, OrderSummaryCacheMiss>();

    private readonly System.Timers.Timer _timer;
    private const int Interval = 5 * 60 * 1000; // 5 minutes

    public CachingService()
    {
        _timer = new System.Timers.Timer(Interval);
        _timer.Elapsed += TimerElapsed;
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _orderSummaryCache.Clear();
    }

    public async Task<OrderSummary> GetOrder(Guid orderId)
    {
        if (_orderSummaryCache.TryGetValue(orderId, out var orderSummary))
        {
            return orderSummary;
        }

        return await GetOrderFromDatabase(orderId).ConfigureAwait(false);
    }

    /// <summary>
    /// Method which gets data from the Database
    /// Checks for previous cache miss if it was recently checked.
    /// Then tries to get the order from the database
    /// </summary>
    /// <returns></returns>
    private async Task<OrderSummary> GetOrderFromDatabase(Guid orderId)
    {
        if (_missingOrderSummaries.TryGetValue(orderId, out var missingOrder))
        {
            if (missingOrder.OrderSummary != null)
            {
                return await Task.FromResult(missingOrder.OrderSummary);
            }

            if ((DateTime.UtcNow - missingOrder.LastChecked).TotalSeconds < 60)
            {
                throw new NotFoundException($"No Order found for order Id: {orderId}");
            }
        }

        try
        {
            //In real-time it should get this data from the database
            var orderSummary = new OrderSummary(new OrderBase(), orderId, OrderStatus.Confirmed);
            _orderSummaryCache.Add(orderSummary.OrderId, orderSummary);
            return await Task.FromResult(orderSummary);
        }
        catch (Exception)
        {
            _missingOrderSummaries[orderId] = new OrderSummaryCacheMiss
            {
                LastChecked = DateTime.UtcNow,
                OrderSummary = null
            };
            throw;
        }
    }
}