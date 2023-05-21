using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestaurantAggregator.Core.Data.DTO;
using SignalRAuthenticationSample.Hubs;

namespace RestaurantAggregator.Notifications.Services;

public class NotificationReciever : BackgroundService
{
    private readonly IConnection _rabbitMqConnection;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationReciever> _logger;

    public NotificationReciever(IConnection rabbitMqConnection, IServiceProvider serviceProvider, ILogger<NotificationReciever> logger)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var channel = _rabbitMqConnection.CreateModel();
        channel.QueueDeclare(queue: "order-status",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var notification = JsonSerializer.Deserialize<OrderStatusNotification>(Encoding.UTF8.GetString(body));
#nullable disable
            _logger.LogInformation("Received message: {OrderNumber} {Status}",
             notification.OrderNumber, notification.Status);
            await SendMessageToHub(notification);
#nullable enable
        };
        channel.BasicConsume(queue: "order-status",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    private async Task SendMessageToHub(OrderStatusNotification notification)
    {
        var hubContext = _serviceProvider.GetRequiredService<IHubContext<NotificationsHub>>();
        await hubContext.Clients.User(notification.ClientId.ToString()).SendAsync("ReceiveMessage",
            $"{DateTime.UtcNow:s} UTC: {notification.OrderNumber} {notification.Status}");
    }
}