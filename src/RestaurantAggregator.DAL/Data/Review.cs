namespace RestaurantAggregator.DAL.Data;
#nullable disable

public class Review
{
    public Guid Id { get; set; }
    public uint Value { get; set; }
    public Guid DishId { get; set; }
    public Guid ClientId { get; set; }
}
