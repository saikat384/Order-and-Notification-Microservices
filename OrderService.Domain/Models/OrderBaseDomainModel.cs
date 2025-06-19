using ECommerceSystem.Base.Models;

namespace OrderService.Domain.Models;

public class OrderBaseDomainModel
{
    public Guid OrderId { get; set; }
    public int CustomerId { get; set; }
    public List<ProductItemDetails>? ProductItemList { get; set; }
    public DateTime Timestamp { get; set; }
}