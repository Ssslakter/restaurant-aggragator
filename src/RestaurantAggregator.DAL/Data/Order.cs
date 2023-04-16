using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.DAL.Data;
#nullable disable

public class Order
{
    public Guid Id { get; set; }
    public uint OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime DeliveryTime { get; set; }
    public string Address { get; set; }
    public decimal TotalPrice { get; set; }
    public List<DishInCart> Dishes { get; set; }
    public Guid ClientId { get; set; }
    public Guid? CourierId { get; set; }
    public Guid? CookId { get; set; }
    public Guid RestaurantId { get; set; }
}