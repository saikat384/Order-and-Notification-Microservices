namespace ECommerceSystem.Base.Models;

public class OrderBase
{
    public int CustomerId { get; set; }
    public List<ProductItemDetails>? ProductItemList { get; set; }
    public DateTime Timestamp { get; set; }
}