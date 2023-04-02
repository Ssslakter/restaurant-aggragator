namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class CartDTO
{
    public ICollection<DishInCart> Dishes { get; set; }
}

public class DishInCart
{
    public Dish Dish { get; set; }
    public uint Count { get; set; }
}