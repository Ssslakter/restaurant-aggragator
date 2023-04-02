namespace RestaurantAggregator.Core.Data;
#nullable disable

public class Review
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    public Guid DishId { get; set; }
    public Guid UserId { get; set; }
}
