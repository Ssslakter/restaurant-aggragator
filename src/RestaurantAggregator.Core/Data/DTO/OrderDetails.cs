namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class OrderDetails : OrderDTO
{
    public List<DishInOrderDTO> Dishes { get; set; }
}
