namespace RestaurantAggregator.Api.Data.Db
{
#pragma warning disable CS8618
    public class Order
    {
        public DateTimeOffset? DeliveryTime { get; set; }
        public DateTimeOffset? OrderTime { get; set; }
        public double Price { get; set; }
        public String Address { get; set; }
    }
}
