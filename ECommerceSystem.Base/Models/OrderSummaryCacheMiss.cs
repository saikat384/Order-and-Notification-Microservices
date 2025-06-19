namespace ECommerceSystem.Base.Models;

public class OrderSummaryCacheMiss
{
    public DateTime LastChecked { get; set; }

    public OrderSummary? OrderSummary { get; set; }
}