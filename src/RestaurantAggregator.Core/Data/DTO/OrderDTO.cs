using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Api.Data.DTO;
#nullable disable

public class OrderDTO
{
    public Guid Id { get; set; }
    public uint OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime? DeliveryTime { get; set; }
    public string Address { get; set; }
    public double Price { get; set; }
}
