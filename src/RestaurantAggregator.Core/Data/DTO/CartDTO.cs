namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class CartDTO
{
    public ICollection<DishInCart> Dishes { get; set; }
}

public class DishInCart
{
    public DishDTO Dish { get; set; }
    public uint Count { get; set; }
}