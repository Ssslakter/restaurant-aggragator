namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class OrderWithDishes : OrderDTO
{
    public List<DishInOrderDTO> Dishes { get; set; }
}
