using RestaurantAggregator.Core.Data.DTO;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace RestaurantAggregator.BL.Services;

public interface INotificationService
{
    void SendOrderStatusNotification(OrderStatusNotification notification);
}

public class NotificationService : INotificationService
{
    private readonly IConnection _rabbitMqConnection;

    public NotificationService(IConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public void SendOrderStatusNotification(OrderStatusNotification notification)
    {
        using var channel = _rabbitMqConnection.CreateModel();
        channel.QueueDeclare(queue: "order-status",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(notification));

        channel.BasicPublish(exchange: "",
            routingKey: "order-status",
            basicProperties: properties,
            body: body);
    }
}