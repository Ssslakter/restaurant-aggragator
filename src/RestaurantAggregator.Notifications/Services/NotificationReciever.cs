using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestaurantAggregator.Core.Data.DTO;
using SignalRAuthenticationSample.Hubs;

namespace RestaurantAggregator.Notifications.Services;

public class NotificationReciever
{
    private readonly IConnection _rabbitMqConnection;
    private readonly IServiceProvider _serviceProvider;

    public NotificationReciever(IConnection rabbitMqConnection, IServiceProvider serviceProvider)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _serviceProvider = serviceProvider;
        AddRabbitListener();
    }

    public void AddRabbitListener()
    {
        using var channel = _rabbitMqConnection.CreateModel();
        channel.QueueDeclare(queue: "order-status",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var notification = JsonSerializer.Deserialize<OrderStatusNotification>(Encoding.UTF8.GetString(body));
#nullable disable
            SendMessageToHub(notification);
#nullable enable
        };
    }

    private void SendMessageToHub(OrderStatusNotification notification)
    {
        var hubContext = _serviceProvider.GetRequiredService<IHubContext<NotificationsHub>>();
        hubContext.Clients.User(notification.ClientId.ToString()).SendAsync("ReceiveMessage",
            $"{DateTime.UtcNow:s} UTC: {notification.OrderNumber} {notification.Status}");
    }
}