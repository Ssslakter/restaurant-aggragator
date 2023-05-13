namespace RestaurantAggregator.Infra.Config;
#nullable disable
public class RabbitConfiguration
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
}
