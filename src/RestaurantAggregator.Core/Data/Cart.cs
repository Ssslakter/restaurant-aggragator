namespace RestaurantAggregator.Core.Data;
#nullable disable
public class Cart
{
    public ICollection<DishInCart> Dishes { get; set; }
}

public class DishInCart
{
    public Dish Dish { get; set; }
    public uint Count { get; set; }
}