namespace RestaurantAggregator.DAL.Data;
#nullable disable
public class Notification
{
    public Guid UserId { get; set; }
    public string Text { get; set; }
    public Guid OrderId { get; set; }
}
