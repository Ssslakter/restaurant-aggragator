namespace RestaurantAggregator.Core.Data;
#nullable disable

public class Rating
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    public Guid DishId { get; set; }
    public Dish Dish { get; set; }
    public Guid UserId { get; set; }
}
